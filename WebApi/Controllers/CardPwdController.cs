using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SDK.Response.CardPwd;
using SDK;
using Utility;
using SDK.Request.CredPwd;
using System.Net;
using SDK.Enum;
using System.Security.Cryptography;


namespace WebApi.Controllers
{
    /// <summary>
    /// 卡密控制器
    /// </summary>
    [Route("api/[controller]")]
    public class CardPwdController : BaseController
    {
        /// <summary>
        /// 获取卡密列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetList")]
        public async Task<IActionResult> GetList()
        {
            const string url = "http://gw.api.agiso.com/acpr/CardPwd/GetList";
            try
            {
                var req = new CardPwdRequest
                {
                    FilterName = string.Empty,
                    ClassId = null,//这个是卡种分类编号，在阿奇索平台上是没有展示，需要浏览器按F12查看接口返回的接口数据（IdNo）字段，60234是开发测试分类的编号
                };

                var dic = await ApiClient.GetDictionaryByObject(req);

                var formDataList = dic.Select(item => new KeyValuePair<string, string?>(item.Key, item.Value.TryString())).ToList();

                var result = await ApiClient.Request<BaseResponse<BasePagerResponse<CardGetListResponse>>>(url, HttpMethod.Post, formDataList);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 获取手动提卡
        /// </summary>
        /// <returns></returns>
        [HttpGet("HandPick")]
        public async Task<IActionResult> HandPick(int? num)
        {
            const string url = "http://gw.api.agiso.com/acpr/CardPwd/HandPick";
            try
            {
                var req = new HandPickRequest
                {
                    CpkId = (int)CpkIdEnum.唯一卡种编号,
                    Num = num,
                    HandPickOrderId = Guid.NewGuid().ToString("n"),
                    Buyer = "空军一号",
                    BuildCpd = false,
                    UsePriority = true,
                };

                var dic = await ApiClient.GetDictionaryByObject(req);

                var formDataList = dic.Select(item => new KeyValuePair<string, string?>(item.Key, item.Value.TryString())).ToList();

                var result = await ApiClient.Request<BaseResponse<HandPickResponse>>(url, HttpMethod.Post, formDataList);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 获取手动加卡
        /// </summary>
        /// <returns></returns>
        [HttpGet("StockIn")]
        public async Task<IActionResult> StockIn()
        {
            const string url = "http://gw.api.agiso.com/acpr/CardPwd/StockIn";
            try
            {
                //var s = new string('B', 20);

                var dataList = Enumerable.Range(1, 100).Select(_ => new Data700And820And840()).ToList();

                // 并行处理每个元素
                Parallel.ForEach(dataList, num =>
                {
                    num.Pwd = GenerateUniquePassword(8).Result;
                    num.CardNo = GenerateUniqueAccount(8).Result;
                });

                var listGroup = dataList.GroupBy(x => x.CardNo).Select(x=> new { x.Key, Count = x.Count()}).ToList();
                var cardNoCount = listGroup.Count(x => x.Count > 1);
                if (cardNoCount > 1)
                {
                    return BadRequest("卡号重复");
                }


                //创建入库批次号,一个批次号失效的时间为半个小时，需要使用的话，重新请求
                var batchId = await CreateStockInBatchId(new CreateStockInBatchIdRequest
                {
                    CpkId = (int)CpkIdEnum.唯一卡种编号,//唯一卡种编号
                    Price = 45,
                    Summary = "接口创建入库批次号(唯一卡种)",
                });

                if (string.IsNullOrWhiteSpace(batchId))
                {
                    return BadRequest("批次号创建失败");
                }

                var req = new StockInRequest
                {
                    StockInBatchId = batchId,
                    Data = JsonSerializer.Serialize(dataList),//这里必须要使用JsonSerializer.Serialize，因为不能使用驼峰命名,否则会接口返回的是 null
                    ExpireTime = DateTime.Now.AddDays(200)
                };

                var dic = await ApiClient.GetDictionaryByObject(req);

                var formDataList = dic.Select(item => new KeyValuePair<string, string?>(item.Key, item.Value.TryString())).ToList();

                var result = await ApiClient.Request<BaseResponse<StockInResponse>>(url, HttpMethod.Post, formDataList);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 获取手动加卡(上传图片)
        /// <para>
        /// 1，图片必须是互联网图片地址，不能是本地地址，否则在阿奇索网站上无法查看图片
        /// 2，文件名称可以任意填写
        /// 3，阿奇索判断图片是否存在是通过MD5值来判断的，所以如果你上传的图片和之前上传过的图片MD5值相同，那么就会提示图片已经存在（这个是重点）
        /// 4，如果你上传的图片和之前上传过的图片地址是一样的，但是手动修改了之前的MD5值，同样也能上传成功（这个也是重点）
        /// </para>
        /// </summary>
        /// <param name="filePathUri">图片路径（必须是互联网图片地址）</param>
        /// <param name="fileName">图片名称（完全可以自定义，比如，龙的传人，御龙在天）</param>
        /// <returns></returns>
        [HttpGet("StockInImage")]
        public async Task<IActionResult> StockInImage(string filePathUri, string fileName)
        {
            const string url = "http://gw.api.agiso.com/acpr/CardPwd/StockIn";
            try
            {
                var dataList = new List<Data850>();
                using var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, filePathUri);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                if (response is { IsSuccessStatusCode: true, StatusCode: HttpStatusCode.OK })
                {
                    var responseBytes = await response.Content.ReadAsByteArrayAsync();
                    //这里计算图片的MD5值，也可以使用Guid.NewGuid().ToString("n")来代替
                    var md5String = SecurityHelper.Md5Encrypt(responseBytes);
                    dataList.Add(new Data850
                    {
                        Url = filePathUri, 
                        Md5 = md5String,
                        OriName = fileName
                    });
                }
                else
                {
                    return BadRequest("图片地址错误");
                }

                //创建入库批次号,一个批次号失效的时间为半个小时，需要使用的话，重新请求
                var batchId = await CreateStockInBatchId(new CreateStockInBatchIdRequest
                {
                    CpkId = (int)CpkIdEnum.图片卡种编号,//图片卡种编号
                    Price = new decimal(25.545323),
                    Summary = "接口创建入库批次号(图片卡种)",
                });

                if (string.IsNullOrWhiteSpace(batchId))
                {
                    return BadRequest("批次号创建失败");
                }

                var req = new StockInRequest
                {
                    StockInBatchId = batchId,
                    Data = JsonSerializer.Serialize(dataList), //这里必须要使用JsonSerializer.Serialize，因为不能使用驼峰命名,否则会接口返回的是null
                    ExpireTime = DateTime.Now.AddDays(200)
                };

                var dic = await ApiClient.GetDictionaryByObject(req);

                var formDataList = dic.Select(item => new KeyValuePair<string, string?>(item.Key, item.Value.TryString())).ToList();

                var result = await ApiClient.Request<BaseResponse<StockInResponse>>(url, HttpMethod.Post, formDataList);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 创建入库批次号,一个批次号失效的时间为半个小时，需要使用的话，重新请求
        /// <para>
        /// 1，唯一卡种编号：1987073
        /// 2，图片卡种编号：2002359
        /// </para>
        /// </summary>
        /// <returns></returns>
        [HttpGet("CreateStockInBatchId")]
        public async Task<IActionResult> CreateStockInBatchId()
        {
            const string url = "http://gw.api.agiso.com/acpr/CardPwd/CreateStockInBatchId";
            try
            {
                var req = new CreateStockInBatchIdRequest
                {
                    CpkId = 2002359,//图片卡种编号，1987073(唯一卡种编号),
                    Price = new decimal(35.8925),
                    Summary = "接口创建入库批次号(图片卡种)",
                };

                var dic = await ApiClient.GetDictionaryByObject(req);

                var formDataList = dic.Select(item => new KeyValuePair<string, string?>(item.Key, item.Value.TryString())).ToList();

                var result = await ApiClient.Request<BaseResponse<string>>(url, HttpMethod.Post, formDataList);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 查询开放平台余额
        /// </summary>
        /// <returns></returns>
        [HttpGet("QueryDeposit")]
        public async Task<IActionResult> QueryDeposit()
        {
            const string url = "http://gw.api.agiso.com/open/Bankroll/QueryDeposit";
            try
            {
                var req = new QueryDepositRequest
                {
                    AppId = "2025041146459120556"
                };
                var dic = await ApiClient.GetDictionaryByObject(req);

                var formDataList = dic.Select(item => new KeyValuePair<string, string?>(item.Key, item.Value.TryString())).ToList();

                var result = await ApiClient.Request<BaseResponse<decimal>>(url, HttpMethod.Post, formDataList);

                var testQuot = new
                {
                    str = $"new List<string>'name',{DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                    str2 = "<string>" + result + "</string>",
                };
                var tt = testQuot.SerializeString();
                return Ok(tt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region 私有方法

        /// <summary>
        /// 根据不同的请求参数，创建入库批次号
        /// 1，唯一卡种编号：1987073
        /// 2，图片卡种编号：2002359</summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static async Task<string> CreateStockInBatchId(CreateStockInBatchIdRequest request)
        {
            const string url = "http://gw.api.agiso.com/acpr/CardPwd/CreateStockInBatchId";
            try
            {
                var dic = await ApiClient.GetDictionaryByObject(request);

                var formDataList = dic.Select(item => new KeyValuePair<string, string?>(item.Key, item.Value.TryString())).ToList();

                var result = await ApiClient.Request<BaseResponse<string>>(url, HttpMethod.Post, formDataList);
                return result is { IsSuccess: true } ? result.Data : string.Empty;
            }
            catch (Exception ex)
            {
                SerilogHelper.Error(ex, "创建入库批次号出错，" + ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// 根据 Take 参数生成不重复密码
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static async Task<string> GenerateUniquePassword(int take)
        {
            try
            {
                const string allowedChars = "ABCDEFGHIJKLMNPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456789!@#$%^&*";

                var randomNumber = RandomNumberGenerator.Create();
                // 生成一个包含所有可能字符的数组
                var uniqueChars = allowedChars.Distinct().ToArray();
                if (uniqueChars.Length < take)
                    throw new InvalidOperationException($"字符池不足以生成{take}位不重复密码。");
                // 使用加密随机数生成器进行洗牌
                for (var i = uniqueChars.Length - 1; i > 0; i--)
                {
                    var randomBytes = new byte[4];
                    randomNumber.GetBytes(randomBytes);
                    var j = Math.Abs(BitConverter.ToInt32(randomBytes, 0)) % (i + 1);
                    (uniqueChars[i], uniqueChars[j]) = (uniqueChars[j], uniqueChars[i]);
                }

                var password = new string(uniqueChars.Take(take).ToArray());
                return await Task.FromResult(password);
            }
            catch (Exception)
            {
                //如果有错误，则返回需要执行位数的密码
                return new string('8', take);
            }

        }

        /// <summary>
        /// 根据 Take 参数生成不重复账号
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static async Task<string> GenerateUniqueAccount(int take)
        {
            try
            {
                const string allowedChars = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";

                var randomNumber = RandomNumberGenerator.Create();
                // 生成一个包含所有可能字符的数组
                var uniqueChars = allowedChars.Distinct().ToArray();
                if (uniqueChars.Length < take)
                    throw new InvalidOperationException($"字符池不足以生成{take}位不重复密码。");
                // 使用加密随机数生成器进行洗牌
                for (var i = uniqueChars.Length - 1; i > 0; i--)
                {
                    var randomBytes = new byte[4];
                    randomNumber.GetBytes(randomBytes);
                    var j = Math.Abs(BitConverter.ToInt32(randomBytes, 0)) % (i + 1);
                    (uniqueChars[i], uniqueChars[j]) = (uniqueChars[j], uniqueChars[i]);
                }

                var password = new string(uniqueChars.Take(take).ToArray());
                return await Task.FromResult(password);
            }
            catch (Exception)
            {
                return new string('A', take); 
            }
        }

        #endregion

    }
}

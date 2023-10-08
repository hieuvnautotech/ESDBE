using Microsoft.AspNetCore.Mvc;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos.Slitting;
using ESD.Services;
using ESD.Services.APP;
using ESD.Services.Common;
using System.Text;

namespace ESD.Controllers.APP
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyerQRController : ControllerBase
    {
        private readonly IBuyerQRService _buyerQRService;
        private readonly ICustomService _customService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly IJwtService _jwtService;
        public BuyerQRController(IBuyerQRService buyerQRService, ICustomService customService, ICommonMasterService commonMasterService, IJwtService jwtService)
        {
            _buyerQRService = buyerQRService;
            _customService = customService;
            _commonMasterService = commonMasterService;
            _jwtService = jwtService;
        }

        [HttpGet]
        [PermissionAuthorization(PermissionConst.BUYERQR_READ)]
        public async Task<IActionResult> GetAll([FromQuery] BuyerQRDto model)
        {
            var returnData = await _buyerQRService.GetAll(model);
            return Ok(returnData);
        }

        [HttpDelete("delete")]
        [PermissionAuthorization(PermissionConst.BUYERQR_DELETE)]
        public async Task<IActionResult> Delete([FromBody] BuyerQRDto model)
        {
            var result = await _buyerQRService.Delete(model);
            return Ok(result);
        }

        [HttpPost("create")]
        [PermissionAuthorization(PermissionConst.BUYERQR_CREATE)]
        public async Task<IActionResult> Create([FromBody] BuyerQRDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            if (model.Stamps.Equals("SDV3"))
            {
                model.QuantityFormat = await ProductQuantityFormatForSDV3(model.PackingAmount);
            }
            else
            {
                model.QuantityFormat = await ProductQuantityFormatForBuyerQR(model.PackingAmount);

            }
            var result = await _buyerQRService.CreateBuyerQR(model);

            return Ok(result);
        }
        private async Task<string> ProductQuantityFormatForBuyerQR(int quantity)
        {
            string str = quantity.ToString();
            int length = str.Length;

            if (length % 2 == 0)
            {
                if (length == 4)
                {
                    string fisrtTwoCharacters = str.Substring(0, 2);
                    string lastTwoCharacter = str.Substring(2, 2);

                    char a =  ChangeNumberToCharacter(int.Parse(fisrtTwoCharacters));
                    StringBuilder result = new StringBuilder();
                    result.Append(a.ToString())
                        .Append(lastTwoCharacter);
                    str = result.ToString();
                }
                else
                {
                    str = string.Concat("0", str);
                }
            }
            else
            {
                if (length == 5)
                {
                    string fisrtTwoCharacters = str.Substring(0, 2);
                    string secondTwoCharacter = str.Substring(2, 2);
                    string lastCharacter = str.Substring(4, 1);

                    char a =  ChangeNumberToCharacter(int.Parse(fisrtTwoCharacters));
                    char b =  ChangeNumberToCharacter(int.Parse(secondTwoCharacter));
                    StringBuilder result = new StringBuilder();
                    result.Append(a.ToString())
                        .Append(b.ToString())
                        .Append(lastCharacter);
                    str = result.ToString();
                }
                if (length == 1)
                {
                    str = string.Concat("00", str);
                }
            }
            return str;
        }
        private async Task<string> ProductQuantityFormatForSDV3(int quantity)
        {
            StringBuilder str = new StringBuilder();
            int x = 0;
            int y = 0;
            int z = 0;

            x = (int)Math.Floor((float)quantity / (32 * 32));
            y = (int)Math.Floor((float)(quantity - x * 32 * 32) / 32);
            z = (int)Math.Floor((float)(quantity - (x * 32 * 32) - (y * 32)));
            var r= str.Append(ChangeNumberToCharacter(x).ToString()).Append(ChangeNumberToCharacter(y).ToString()).Append(ChangeNumberToCharacter(z).ToString()).ToString();
            return r;
        }

        private char ChangeNumberToCharacter(int number)
        {
            string temp = number.ToString();
            char result = 'A';
            int subtraction = 0;
            if (number < 10)
            {
                return char.Parse(temp);
            }
            else
            {
                subtraction = number - 10;
                for (int i = 0; i < subtraction; i++)
                {
                    result++;
                }
            }
            return result;
        }
        [HttpPost("get-print")]
        public async Task<IActionResult> GetListPrintQR([FromBody] List<long> listQR)
        {
            return Ok(await _buyerQRService.GetListPrintQR(listQR));
        }


        [HttpGet("get-product")]
        public async Task<IActionResult> GetProduct()
        {
            string Column = "ProductId, concat(ProductCode, ' - ', ProductName) as ProductLabel , ProductCode, Vendor, PackingAmount,Stamps, RemarkBuyer";
            string Table = "Product";
            string Where = "isActived = 1 and Stamps <> 'INOX' and Vendor is not null and PackingAmount is not null";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-product-inox")]
        public async Task<IActionResult> GetProductInox()
        {
            string Column = "ProductId, ProductCode, Vendor, PackingAmount,Stamps, RemarkBuyer";
            string Table = "Product";
            string Where = "isActived = 1 and Stamps = 'INOX' and Vendor is not null and PackingAmount is not null";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
    }
}

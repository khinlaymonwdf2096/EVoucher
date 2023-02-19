using AutoMapper;
using CodeTest.DTO;
using CodeTest.IServices;
using CodeTest.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CodeTest.Controllers.CMS
{
    public class CMSReportController : Controller
    {

        private readonly ILogger<CMSReportController> _logger;

        private readonly IMapper _mapper;

        private readonly IPOSService _posService;

        public CMSReportController(ILogger<CMSReportController> logger, IMapper mapper, IPOSService posService)
        {
            _logger = logger;
            _mapper = mapper;
            _posService = posService;
        }

        public async Task<IActionResult> Index()
        {
            List<Purchase> purchases = await _posService.GetAllPurchases();
            List<PurchaseDTO> purchaseDtos = new List<PurchaseDTO>();
            purchases.ForEach(x => purchaseDtos.Add(_mapper.Map<PurchaseDTO>(x)));

            return View("CMSReportView",purchaseDtos);
            //return View("CMSReportView");
        }

        //public List<CouponHistory> GetCouponHistory()
        //{

        //    string connectionString = "User ID =postgres;Password=015427;Server=localhost;Port=5432;Database=CouponDb";
        //    string result = null;
        //    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            NpgsqlCommand command = new NpgsqlCommand();

        //            command.CommandText = query;
        //            command.CommandTimeout = 3600;
        //            command.CommandType = System.Data.CommandType.Text;
        //            command.Connection = connection;

        //            connection.Open();

        //            var tempResult = await command.ExecuteScalarAsync();
        //            result = tempResult == null ? null : Convert.ToString(tempResult);

        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //}
    }
}

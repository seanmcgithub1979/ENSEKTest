using System;
using System.IO;
using System.Linq;
using ENSEK.Metering.Domain.Models;
using ENSEK.Metering.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ENSEK.Metering.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeterReadingController : ControllerBase
    {
        private readonly ILogger<MeterReadingController> _logger;
        private readonly IValidationService _validationService;
        private readonly IDataService _dataService;

        public MeterReadingController(
            IValidationService validationService,
            IDataService dataService,
            ILogger<MeterReadingController> logger)
        {
            _logger = logger;
            _validationService = validationService;
            _dataService = dataService;
        }

        [HttpPost]
        [Route("/meter-reading-uploads")]
        public IActionResult Index()
        {
            var meterReadingUploadResponse = new MeterReadingUploadResponse();
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var file = Request.Form.Files[0];

            if (file.Length != 0)
            {
                var fullPath = Path.Combine(baseDir, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                if (!_validationService.IsValidFile(fullPath))
                {
                    return new JsonResult("Not a csv file.");
                }

                var lines = System.IO.File.ReadAllLines(fullPath);

                var dataLines = lines.Skip(1); // Skip column headers
                foreach (var line in dataLines)
                {
                    var rowData = line.Split(',');

                    var accountId = rowData[0];
                    if (_validationService.IsValidRow(rowData))
                    {
                        // Dat is valid as this point
                        var meterReading = int.Parse(rowData[2]);
                        if (!_validationService.IsValidReading(meterReading))
                        {
                            meterReadingUploadResponse.MeterReadingUploadResults.Add(new MeterReadingUploadResult
                            {
                                AccountId = accountId,
                                Status = "FAILURE",
                                AdditionalInfo = $"Meter reading must be in the range 10000-99999 - Meter reading supplied was {meterReading}"
                            });

                            continue;
                        }

                        if (_dataService.Save(rowData) > 0)
                        {
                            meterReadingUploadResponse.MeterReadingUploadResults.Add(new MeterReadingUploadResult
                            {
                                AccountId = accountId,
                                Status = "SUCCESS",
                                AdditionalInfo = $"Saved to the DB - AccountId {accountId}"
                            });
                        }
                        else
                        {
                            meterReadingUploadResponse.MeterReadingUploadResults.Add(new MeterReadingUploadResult
                            {
                                AccountId = accountId,
                                Status = "FAILURE",
                                AdditionalInfo = $"Unable to save to the DB - AccountId {accountId}"
                            });
                        }
                    }
                    else
                    {
                        meterReadingUploadResponse.MeterReadingUploadResults.Add(new MeterReadingUploadResult
                        {
                            AccountId = accountId,
                            Status = "FAILURE",
                            AdditionalInfo = $"Invalid data in rowData - Data supplied was {rowData[0]} {rowData[1]} {rowData[2]}"
                        });
                    }
                }
            }
            else
            {
                return new JsonResult("Empty file.");
            }

            return new JsonResult(meterReadingUploadResponse.MeterReadingUploadResults);
        }
    }
}

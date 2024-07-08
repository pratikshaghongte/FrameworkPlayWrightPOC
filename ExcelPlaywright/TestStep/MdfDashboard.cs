using AventStack.ExtentReports;
using ExcelPlaywright.Utils;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelPlaywright.TestStep
{
    internal class MdfDashboard : TestBase
    {
        private readonly TestUtils _testUtils;

        public MdfDashboard(TestUtils testUtils)
        {
            _testUtils = testUtils;
        }

        private string btnCreateRequest = "//button[contains(.,'Create Request')]";
        private string lblSuccessMsg = "//div[@class='k-notification-content']";

        internal async Task ClickOnCreateFundRequest()
        {
            await _testUtils.WaitForMovement(btnCreateRequest);
            await _testUtils.WaitForMovement(btnCreateRequest);
            await _testUtils.Click(btnCreateRequest);
        }

        internal async Task VerifySuccessMessageOfFundRequestCreated(string fundRequestNumber)
        {
            await _testUtils.WaitForMovement(lblSuccessMsg);

            //if (await _testUtils.WaitForMovementt(lblSuccessMsg))
            //{
                _test.Log(Status.Info, "Verify fund request created success message is displayed");
                string expectedMsg = "Your fund request: " + fundRequestNumber + " is pending approval";
                string actualMsg = await _testUtils.GetTextFromElementAsync(lblSuccessMsg);
                await _testUtils.AssertVerifyAsync(actualMsg, expectedMsg);
            //}
            //else
            //{
                _test.Log(Status.Skip, "Skipped verification of fund request created");
           // }
        }
    }
}

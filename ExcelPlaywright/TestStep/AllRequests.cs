using AventStack.ExtentReports;
using ExcelPlaywright.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelPlaywright.TestStep
{
    internal class AllRequests : TestBase
    {
        private readonly TestUtils _testUtils;

        public AllRequests(TestUtils testUtils)
        {
            _testUtils = testUtils;
        }
        private string menuManage = "//div[@id='ctl00_radMainMenu_radMenuMain']/ul/li/a/span[.='Manage']";
        private string txtSearchRequestName = "//input[@name='ctl00_ContentPlaceHolderBody_gFundRequests_ctl00_ctl02_ctl03_rsbFundRequestsFundRequestName']";
        private string txtSearchRequestId = "//input[@name='ctl00_ContentPlaceHolderBody_gFundRequests_ctl00_ctl02_ctl03_rsbFundRequestsFundRequestNumber']";
        private string btnSearchRequest = "(//input[@value='Type To Search'])[1]/following-sibling::button";

   
        string data;

        internal async Task searchFundRequestId()
        {
            data = await TestUtils.GetGlobalRecordAsync();

            var txtSearchRequestIdElement = await _page.QuerySelectorAsync(txtSearchRequestId);
            if (txtSearchRequestIdElement != null)
            {
                await txtSearchRequestIdElement.TypeAsync(data);
            }

            var btnSearchElement = await _page.QuerySelectorAsync("selector_for_btnSearch");
            if (btnSearchElement != null)
            {
                await btnSearchElement.ClickAsync();
            }
        }

    
    }
}

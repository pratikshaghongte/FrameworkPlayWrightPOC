using ExcelPlaywright.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelPlaywright.TestStep
{
    public class Channel_Mechanics_Interface : TestBase
    {
        private readonly TestUtils _testUtils;

        public Channel_Mechanics_Interface(TestUtils testUtils)
        {
            _testUtils = testUtils;
        }

        private string menuManage = "//div[@id='ctl00_radMainMenu_radMenuMain']/ul/li/a/span[.='Manage']";
        private string subMenuManageVendor = "//div[@id='ctl00_radMainMenu_radMenuMain']/ul/li/a/span[.='Manage']";
        private string menuModules = "//div[@id='ctl00_radMainMenu_radMenuMain']/ul/li/a/span[.='Modules']";
        private string subMenuFundIT = "//span[contains(text(),'fundIT') and @class='rmText rmExpandRight']";
        private string superSubMenuMyPendingRequestfundIT = "(//span[@class='rmText'][normalize-space()='My Pending Requests'])[4]";
       
        public async Task NavigateOnManageVendors()
        {
            await _testUtils.Click(menuManage);
            await _testUtils.Click(subMenuManageVendor);
        }

        public async Task ClickOnModules()
        {
            await _testUtils.Click(menuModules);
        }

        public async Task ClickOnFundIT()
        {
            await _testUtils.Click(subMenuFundIT);
        }
        public async Task ClickOnMyPendingRequest()
        {
            await _testUtils.Click(superSubMenuMyPendingRequestfundIT);

        }
    }
}

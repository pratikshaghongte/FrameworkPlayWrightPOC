using ExcelPlaywright.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelPlaywright.TestStep
{
    public class ManageUsers : TestBase
    {
        private readonly TestUtils _testUtils;

        public ManageUsers(TestUtils testUtils)
        {
            _testUtils = testUtils;
        }

        private string menuManage = "//div[@id='ctl00_radMainMenu_radMenuMain']/ul/li/a/span[.='Manage']";
        private string subMenuManageUsers = "//span[contains(text(),'Manage Users') and @class='rmText']";
        private string txtSearchUserName = "#txtUserName";
        private string btnSearch = "//span[@id='ctl00_ContentPlaceHolderBody_btnSearch']";
        private string btnPlug = "(//td//a//img)[1]";

        public async Task NavigateToManageUsers()
        {
            await _testUtils.Click(menuManage);
            await _testUtils.Click(subMenuManageUsers);
        }

        public async Task VerifyUserCanLoginAsRSU()
        {
            string userName = TestUtils.GetDataByKey("RSUUserName");
            await _testUtils.FillField(txtSearchUserName, userName); // Ensure txtSearchUserName is properly defined and corresponds to the element in your page
                                                                     //return userName;
            await _testUtils.Click(btnSearch);
            await _testUtils.Click(btnPlug);
        }

        internal async Task verifyUserCanLoginAsVSU()
        {
            string userName = TestUtils.GetDataByKey("UserName");
            await _testUtils.FillField(txtSearchUserName, userName); // Ensure txtSearchUserName is properly defined and corresponds to the element in your page
                                                                     //return userName;
            await _testUtils.Click(btnSearch);
            await _testUtils.Click(btnPlug);
        }
    }
}

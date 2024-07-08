using ExcelPlaywright.Utils;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace ExcelPlaywright.TestStep
{
    public class Login : TestBase
    {
        private readonly TestUtils _testUtils;

        public Login(TestUtils testUtils)
        {
            _testUtils = testUtils;
        }

        private string iFrameLogin = "//*[@id='RadWindowWrapper_rwCookieMessage']/table/tbody/tr[2]/td[2]/iframe";
        private string btnIAgree = "//*[@id='ctl00_ContentPlaceHolderBody_bCookiesAccept_input']";
        private string txtUserName = "#UserName";
        private string txtPassword = "#ctl00_ContentPlaceHolderBody_cmlLogin_Password";
        private string btnLogin = "#ctl00_ContentPlaceHolderBody_cmlLogin_LoginButton2";
        private string menuManage = "//div[@id='ctl00_radMainMenu_radMenuMain']/ul/li/a/span[.='Manage']";
        private string subMenuManageUsers = "//span[contains(text(),'Manage Users') and @class='rmText']";
        private string txtSearchUserName = "#txtUserName";
        private string btnSearch = "//span[@id='ctl00_ContentPlaceHolderBody_btnSearch']";
        private string btnPlug = "(//td//a//img)[1]";
        private string iFrameLogin2 = "//*[@id='RadWindowWrapper_rwCookieMessage']/table/tbody/tr[2]/td[2]/iframe";

        public async Task userLogin()
        {
            // Wait for and switch to the iframe
            await _testUtils.WaitForSelectorStateAsync(_page, iFrameLogin, ElementState.Visible);
            var frame = await _testUtils.SwitchFrameAsync(iFrameLogin);

            // Wait for the button and click it
            await frame.WaitForSelectorAsync(btnIAgree, new FrameWaitForSelectorOptions { State = WaitForSelectorState.Visible });
            await frame.ClickAsync(btnIAgree);

            // Switch back to the default frame
            await _testUtils.SwitchToDefaultFrameAsync();

            await _testUtils.FillField(txtUserName, USERNAME);
            await _testUtils.FillField(txtPassword, PASSWORD);

            // Wait for network to be idle before clicking login
            await _testUtils.WaitForNetworkIdleAsync(_page);
            await _testUtils.Click(btnLogin);
        }

        public async Task NavigateToManageUsers()
        {
            await _testUtils.Click(menuManage);
            await _testUtils.Click(subMenuManageUsers);
        }

        public async Task GetUserCanLoginAsVSU()
        {
            string userName = TestUtils.GetDataByKey("UserName");
            await _testUtils.FillField(txtSearchUserName, userName); // Ensure txtSearchUserName is properly defined and corresponds to the element in your page
                                                                     //return userName;
            await _testUtils.Click(btnSearch);
            await _testUtils.Click(btnPlug);
        }
       
        public async Task IAgreechecked()
        {
            _testUtils.SwitchTabAsync(1);
            await _testUtils.WaitForSelectorStateAsync(_page, iFrameLogin2, ElementState.Visible);
            var frame = await _testUtils.SwitchFrameAsync(iFrameLogin2);

            await frame.WaitForSelectorAsync(btnIAgree, new FrameWaitForSelectorOptions { State = WaitForSelectorState.Visible });
            await frame.ClickAsync(btnIAgree);

            await _testUtils.SwitchToDefaultFrameAsync();
        }
    }
}


















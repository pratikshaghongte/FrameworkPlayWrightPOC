using NUnit.Framework;
using System.Threading.Tasks;
using ExcelPlaywright.TestStep;
using ExcelPlaywright.Utils;
using Microsoft.Playwright;

namespace ExcelPlaywright.Tests
{
    [TestFixture]
    public class UserLoginToLabs : TestBase
    {
        private TestUtils _testUtils;
        private Login _login;

        [OneTimeSetUp]
        public void SetupTest()
        {
            _testUtils = new TestUtils(_page);
            _login = new Login(_testUtils);
            TestUtils.LoadExcelData("End2End.xlsx", "MDFPraposal");
        }

        [Test, Order(1)]
        [Category("LoginTests")]
        public async Task TestLogin()
        {
            _test.Log(AventStack.ExtentReports.Status.Info, "Starting TestLogin");
            _test.Log(AventStack.ExtentReports.Status.Info, "Logging in the user");

            await _login.userLogin();

            _test.Log(AventStack.ExtentReports.Status.Pass, "User logged in successfully");
            _test.Log(AventStack.ExtentReports.Status.Info, "Switching to tab 0");

            await _testUtils.SwitchTabAsync(0);
            await _testUtils.WaitForNetworkIdleAsync(_testUtils.CurrentPage); // Ensure network is idle

            _test.Log(AventStack.ExtentReports.Status.Pass, "Switched to tab 0");
            _test.Log(AventStack.ExtentReports.Status.Info, "Navigating to Manage Users");

            await _login.NavigateToManageUsers();
            await _testUtils.WaitForNetworkIdleAsync(_testUtils.CurrentPage); // Ensure network is idle

            _test.Log(AventStack.ExtentReports.Status.Pass, "Navigated to Manage Users");
            _test.Log(AventStack.ExtentReports.Status.Info, "Getting user who can login as VSU");

            await _login.GetUserCanLoginAsVSU();
            await _testUtils.WaitForNetworkIdleAsync(_testUtils.CurrentPage); // Ensure network is idle

            _test.Log(AventStack.ExtentReports.Status.Info, "User able to click on search");
            _test.Log(AventStack.ExtentReports.Status.Pass, "User clicked on search successfully");
            _test.Log(AventStack.ExtentReports.Status.Info, "Switching to tab 1");

            await _testUtils.SwitchTabAsync(1);
            await _testUtils.WaitForNetworkIdleAsync(_testUtils.CurrentPage); // Ensure network is idle

            _test.Log(AventStack.ExtentReports.Status.Pass, "Switched to tab 1");
            _test.Log(AventStack.ExtentReports.Status.Info, "Checking 'I Agree' checkbox");

            await _login.IAgreechecked();
            await _testUtils.WaitForNetworkIdleAsync(_testUtils.CurrentPage); // Ensure network is idle

            await _testUtils.SwitchTabAsync(0);
            await _testUtils.WaitForNetworkIdleAsync(_testUtils.CurrentPage); // Ensure network is idle

            await _login.NavigateToManageUsers();
            await _testUtils.WaitForNetworkIdleAsync(_testUtils.CurrentPage); // Ensure network is idle

            await _login.GetUserCanLoginAsVSU();
            await _testUtils.WaitForNetworkIdleAsync(_testUtils.CurrentPage); // Ensure network is idle

            _test.Log(AventStack.ExtentReports.Status.Pass, "'I Agree' checkbox checked successfully");

            _test.Log(AventStack.ExtentReports.Status.Info, "Ending TestLogin");
        }

        [Test, Order(2)]
        [Category("AnotherTest")]
        public async Task AnotherTest()
        {
            _test.Log(AventStack.ExtentReports.Status.Info, "Starting AnotherTest");

            // Your test logic here

            _test.Log(AventStack.ExtentReports.Status.Pass, "AnotherTest completed successfully");

            _test.Log(AventStack.ExtentReports.Status.Info, "Ending AnotherTest");
        }

    }
}













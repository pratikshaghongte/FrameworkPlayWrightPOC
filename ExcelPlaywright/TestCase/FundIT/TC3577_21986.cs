using ExcelPlaywright.TestStep;
using ExcelPlaywright.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelPlaywright.TestCase.FundIT
{
    [TestFixture]
    public class TC3577_21986 : TestBase
    {
        private TestUtils _testUtils;
        private Login _login;
        private Channel_Mechanics_Interface _channel_Mechanics_Interface;
        private ManageUsers _manageUsers;
        private Partners _partner;
        private MdfDashboard _mdfDashboard;
        private CreateMdfRequest _createMdfRequest;
        private AllRequests _allRequests;

        [OneTimeSetUp]
        public void SetupTest()
        {
            _testUtils = new TestUtils(_page);
            _login = new Login(_testUtils);
            _channel_Mechanics_Interface = new Channel_Mechanics_Interface(_testUtils);
            _manageUsers = new ManageUsers(_testUtils);
            _partner = new Partners(_testUtils);
            _mdfDashboard = new MdfDashboard(_testUtils);
            _createMdfRequest = new CreateMdfRequest(_testUtils);
            _allRequests = new AllRequests(_testUtils);
            TestUtils.LoadExcelData("End2End.xlsx", "MDFPraposal");
        }

        [Test, Order(1)]
        [Category("RegressionNewUI")]
        public async Task TC3577_21986_VerifyRequestStatusshouldbe_Displayas_RequestApproved_after_Submitting_and_approving_Fund_Request()
        {
            await _login.userLogin();
            string mdfProgramName = TestUtils.GetDataByKey("MDFProgram");
            Console.WriteLine("[DEBUG] Logging in as user...");

            await _testUtils.SwitchTabAsync(0);
            await _manageUsers.NavigateToManageUsers();
            await _manageUsers.VerifyUserCanLoginAsRSU();
            Console.WriteLine("[DEBUG] Verified user can login as RSU...");

            await _testUtils.SwitchTabAsync(1);
            await _partner.IAgreeCheck();
            await _partner.ClickOnMyProgramAndSelectProgram(mdfProgramName);

            Console.WriteLine("[DEBUG] Selected program: " + mdfProgramName);

            await _mdfDashboard.ClickOnCreateFundRequest();

            //Fill Activity Detail -> click on next button

            string category = TestUtils.GetDataByKey("Category1");
            string activitytype = TestUtils.GetDataByKey("ActivityType2ForCategory1");
            string activity = TestUtils.GetDataByKey("Activity2");
            string totalCostOfActivityExcel = TestUtils.GetDataByKey("TotalCostOfActivity");
            string fundRequestedAmountExcel = TestUtils.GetDataByKey("FundRequested");

            await _createMdfRequest.SetRequestName();
            await _createMdfRequest.ClickOnNextButton();
            await _createMdfRequest.SetTotalCostOfActivity(totalCostOfActivityExcel);
            await _createMdfRequest.SetFundRequestedAmount(fundRequestedAmountExcel);
            await _createMdfRequest.ClickOnNextButton();
            await _createMdfRequest.ClickOnNextButton();
            await _createMdfRequest.ClickOnSeePreviewButton();

            //verifiy
            string fundRequestNumber = await _createMdfRequest.StoreFundRequestNumberAsync();
            await _createMdfRequest.ClickOnSubmitRequest();
            await _mdfDashboard.VerifySuccessMessageOfFundRequestCreated(fundRequestNumber);


            //another task
            await _testUtils.SwitchTabAsync(0);
            await _manageUsers.NavigateToManageUsers();
            await _manageUsers.verifyUserCanLoginAsVSU();
            Console.WriteLine("[DEBUG] Verified user can login as VSU...");

            await _testUtils.SwitchTabAsync(1);

            await _channel_Mechanics_Interface.ClickOnModules();
            await _channel_Mechanics_Interface.ClickOnFundIT();
            await _channel_Mechanics_Interface.ClickOnMyPendingRequest();
            await _allRequests.searchFundRequestId();
        }
    }
}

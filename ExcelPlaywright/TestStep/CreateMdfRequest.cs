using ExcelPlaywright.Utils;
using Microsoft.Playwright;

namespace ExcelPlaywright.TestStep
{
    internal class CreateMdfRequest : TestBase
    {
        private readonly TestUtils _testUtils;

        public CreateMdfRequest(TestUtils testUtils)
        {
            _testUtils = testUtils;
        }

        private string txtRequestName = "//input[@name='FundRequestName']";
        private string btnNext = "//button[@type='submit'][contains(.,'Next')]";
        private string txtTotalCostOfActivity = "//input[@id='Activity']";
        private string txtFundRequestedAmount = "//input[@id='Funds']";
        private string btnSeePreview = "//button[contains(.,'See Preview')]";
        private string lblFundRequestNumber = "//span[contains(.,'Fund Request No.')]/following-sibling::div";
        private string btnSubmitRequest = "//button[@type='submit']/span[.='Submit Request ']";
        
        internal async Task SetRequestName()
        {
            await _testUtils.WaitForMovement(txtRequestName);
            await _testUtils.WaitForMovement(txtRequestName);
            await _testUtils.WaitForSelectorStateAsync(_page, txtRequestName, ElementState.Visible);

            string requestName = TestUtils.GetCurrentTime() + TestUtils.GetDataByKey("RequestName");
            await _testUtils.FillField(txtRequestName, requestName);

            TestUtils.DeleteGlobalRecordAsync();
            TestUtils.StoreGlobalRecordAsync(requestName);
            TestContext.WriteLine(requestName);
        }

        internal async Task ClickOnNextButton()
        {
            await _testUtils.WaitForMovement(btnNext);
            await _testUtils.WaitForMovement(btnNext);
            await _testUtils.WaitForSelectorStateAsync(_page, btnNext, ElementState.Visible);
            await _testUtils.Click(btnNext);
        }

        internal async Task SetTotalCostOfActivity(string totalCostOfActivity)
        {
            await _testUtils.WaitForSelectorStateAsync(_page, txtTotalCostOfActivity, ElementState.Visible);
            await _testUtils.FillField(txtTotalCostOfActivity, totalCostOfActivity); // Ensure txtSearchUserName is properly defined and corresponds to the element in your page
        }

        internal async Task SetFundRequestedAmount(string fundRequestedAmount)
        {
            await _testUtils.WaitForSelectorStateAsync(_page, txtFundRequestedAmount, ElementState.Visible);
            await _testUtils.FillField(txtFundRequestedAmount, fundRequestedAmount);
        }

        internal async Task ClickOnSeePreviewButton()
        {
            await _testUtils.WaitForSelectorStateAsync(_page, btnSeePreview, ElementState.Visible);
            await _testUtils.Click(btnSeePreview);
            Thread.Sleep(4000);
        }

        internal async Task<string> StoreFundRequestNumberAsync()
        {
            await _testUtils.WaitForMovement(lblFundRequestNumber);
            await _testUtils.WaitForMovement(lblFundRequestNumber);
            await _testUtils.WaitForSelectorStateAsync(_page, lblFundRequestNumber, ElementState.Visible);

            string fundRequestNumber = await _testUtils.GetTextFromElementAsync(lblFundRequestNumber);

            await TestUtils.DeleteGlobalRecordAsync();
            await TestUtils.StoreGlobalRecordAsync(fundRequestNumber);

            TestContext.WriteLine(fundRequestNumber);
            return fundRequestNumber;
        }

        internal async Task ClickOnSubmitRequest()
        {
            await _testUtils.WaitForMovement(btnSubmitRequest);
            await _testUtils.WaitForSelectorStateAsync(_page, btnSubmitRequest, ElementState.Visible);
            await _testUtils.ScrollToElementAsync(btnSubmitRequest);
            await _testUtils.Click(btnSubmitRequest);
        }
    }
}

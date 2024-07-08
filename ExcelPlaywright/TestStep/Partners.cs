using ExcelPlaywright.Utils;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelPlaywright.TestStep
{
    public class Partners : TestBase
    {
        private readonly TestUtils _testUtils;

        public Partners(TestUtils testUtils)
        {
            _testUtils = testUtils;
        }

        private string ddlMyPrograms = "//li/a[contains(.,'My Programs')]";
        private string btnIAgree = "//div[2]/button/span[contains(text(),'I Agree')]";

        internal async Task ClickOnMyProgramAndSelectProgram(string programName)
        {
            Thread.Sleep(6000);
            await _testUtils.WaitForSelectorStateAsync(_page, ddlMyPrograms, ElementState.Visible);
            await _testUtils.Click(ddlMyPrograms);
            var selectProgram = _page.Locator($"//a[contains(text(), '{programName}')]");
            await selectProgram.First.ClickAsync();  // Click on the first element found
            Thread.Sleep(5000);
        }

        internal async Task IAgreeCheck()
        {
            await _testUtils.SwitchTabAsync(1);
            await _testUtils.WaitForSelectorStateAsync(_page, btnIAgree, ElementState.Visible);
            await _testUtils.Click(btnIAgree);
        }
    }
}

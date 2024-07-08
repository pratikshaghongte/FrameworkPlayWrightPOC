using AventStack.ExtentReports;
using ExcelDataReader;
using Microsoft.Playwright;
using System.Reflection;

namespace ExcelPlaywright.Utils
{
    public class TestUtils : TestBase
    {
        private IPage _originalPage;
        public IPage CurrentPage { get; private set; }

        public TestUtils(IPage page)
        {
            _page = page;
            CurrentPage = page;
        }

        //Click
        public async Task Click(string selector) => await _page.ClickAsync(selector);

        public async Task Click(string selector, float positionX, float positionY) =>
        await _page.Locator(selector).ClickAsync(new() { Position = new Position { X = positionX, Y = positionY } });

        //Double click
        public async Task DoubleClick(string selector) => await _page.DblClickAsync(selector);

        //Fill input fields
        public async Task FillField(string selector, string? value) => await _page.FillAsync(selector, value);

        public async Task<IFrame> SwitchFrameAsync(string frameSelector)
        {
            await _page.WaitForSelectorAsync(frameSelector, new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
            var frameElementHandle = await _page.QuerySelectorAsync(frameSelector);
            if (frameElementHandle == null)
            {
                throw new Exception("Frame element not found.");
            }
            var frame = await frameElementHandle.ContentFrameAsync();
            if (frame == null)
            {
                throw new Exception("Frame not found.");
            }
            return frame;
        }

        public async Task SwitchToDefaultFrameAsync()
        {
            await _page.SetContentAsync(await _page.ContentAsync());
        }


        //Toggle Checkbox
        public async Task ToggleCheckBox(string selector, bool check = true)
        {
            await _page.Locator(selector).SetCheckedAsync(check);
        }

        //ALert Box
        public async Task HandleAlertBoxAsync(string acceptOrReject)
        {
            _page.Dialog += async (_, dialog) =>
            {
                if (acceptOrReject.ToLower() == "accept")
                {
                    await dialog.AcceptAsync();
                }
                else if (acceptOrReject.ToLower() == "reject")
                {
                    await dialog.DismissAsync();
                }
            };
        }

        public async Task ScrollToElementAsync(string selector)
        {
            var element = await _page.QuerySelectorAsync(selector);
            if (element != null)
            {
                await element.ScrollIntoViewIfNeededAsync();
            }
            else
            {
                throw new Exception($"Element with selector {selector} not found.");
            }
        }


        //select by name from dropdown
        public async Task SelectByNameAsync(string selector, string optionText)
        {
            var element = await _page.QuerySelectorAsync(selector);
            if (element != null)
            {
                var option = await element.QuerySelectorAsync($"option:has-text(\"{optionText}\")");
                if (option != null)
                {
                    var value = await option.GetAttributeAsync("value");
                    await element.SelectOptionAsync(new[] { value });
                }
                else
                {
                    throw new Exception($"Option with text '{optionText}' not found.");
                }
            }
            else
            {
                throw new Exception($"Element with selector {selector} not found.");
            }
        }
        public async Task NavigateURL(string url)
        {
            await _page.GotoAsync(url);
        }
        public static string GetCurrentTime()
        {
            return Regex.Replace(DateTime.Now.ToString(), @"[^0-9a-zA-Z]+", "");
        }
        public static async Task MouseOver(IPage page, string selector)
        {
            await page.HoverAsync(selector);
        }
        public async Task WaitForMovement(string selector)
        {
            await _page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions
            {
                State = WaitForSelectorState.Visible
            });
        }

        public static async Task ClickOnButton(IPage page, string selector)
        {
            var button = await page.QuerySelectorAsync(selector);
            if (button != null)
            {
                await button.ClickAsync();
            }
            else
            {
                throw new Exception($"Button with selector '{selector}' not found.");
            }
        }
        public static async Task RightClickAction(IPage page, string selector)
        {
            var element = await page.QuerySelectorAsync(selector);
            if (element != null)
            {
                await element.ClickAsync(new ElementHandleClickOptions { Button = MouseButton.Right });
            }
            else
            {
                throw new Exception($"Element with selector '{selector}' not found.");
            }
        }

        private static string GetProjectPath()
        {
            var path = Assembly.GetExecutingAssembly().Location;
            var actualPath = Path.GetDirectoryName(path);
            var projectPath = Path.Combine(actualPath, "Text1.txt");
            return projectPath;
        }
        
        public async Task<string> GetTextFromElementAsync(string selector)
        {
            var element = await _page.QuerySelectorAsync(selector);
            return await element.InnerTextAsync();
        }

        public static async Task StoreGlobalRecordAsync(string data)
        {
            var projectPath = GetProjectPath();
            try
            {
                await File.WriteAllTextAsync(projectPath, data);
            }
            catch (Exception e)
            {
                TestContext.WriteLine(e);
            }
        }

        public static async Task DeleteGlobalRecordAsync()
        {
            var projectPath = GetProjectPath();
            try
            {
                if (File.Exists(projectPath))
                {
                    File.Delete(projectPath);
                }
            }
            catch (Exception e)
            {
                TestContext.WriteLine(e);
            }
        }

        public static async Task<string> GetGlobalRecordAsync()
        {
            var projectPath = GetProjectPath();
            try
            {
                if (File.Exists(projectPath))
                {
                    var data = await File.ReadAllTextAsync(projectPath);
                    TestContext.Progress.WriteLine(data);
                    return data;
                }
            }
            catch (Exception e)
            {
                TestContext.Progress.WriteLine(e);
            }
            return null;
        }

        // Wait for selector state
        public async Task WaitForSelectorStateAsync(IPage page, string selector, ElementState state, int timeout = 30000)
        {
            var waitOptions = new PageWaitForSelectorOptions
            {
                Timeout = timeout,
                State = state switch
                {
                    ElementState.Visible => WaitForSelectorState.Visible,
                    ElementState.Hidden => WaitForSelectorState.Hidden,
                    ElementState.Enabled => WaitForSelectorState.Visible,
                    ElementState.Disabled => WaitForSelectorState.Attached,
                    _ => WaitForSelectorState.Attached
                }
            };

            var element = await page.WaitForSelectorAsync(selector, waitOptions);
            if (element == null)
            {
                throw new TimeoutException($"Element '{selector}' not found within the specified timeout of {timeout} milliseconds.");
            }
        }

        public static void LoadExcelData(string fileName, string sheetName)
        {
            // Define the path to the Excel file
            var workingDirectory = Environment.CurrentDirectory;
            var projectPath = workingDirectory.Substring(0, workingDirectory.IndexOf("bin"));
            var filePath = Path.Combine(projectPath, "TestData", fileName);

            // Validate the file extension
            if (!filePath.EndsWith(".xlsx"))
            {
                throw new ArgumentException("Input data file must be in .xlsx format");
            }

            // Open the Excel file for reading
            using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            // Read the Excel file into a DataSet
            using var excelReader = ExcelReaderFactory.CreateReader(stream);
            var result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });

            // Get the specified sheet from the DataSet
            var table = result.Tables;
            if (!table.Contains(sheetName))
            {
                throw new ArgumentException($"Sheet '{sheetName}' not found in the Excel file");
            }
            var resultTable = table[sheetName];

            // Load data from the sheet into the dictionary
            testDataFromInput = new Dictionary<string, string>();
            for (int row = 0; row < resultTable.Rows.Count; row++)
            {
                var key = resultTable.Rows[row][0].ToString();
                var value = resultTable.Rows[row][1].ToString();
                testDataFromInput.Add(key, value);
            }
        }
        public static string GetDataByKey(string key)
        {
            if (testDataFromInput.ContainsKey(key))
            {
                return testDataFromInput[key];
            }
            else
            {
                throw new KeyNotFoundException($"The key '{key}' was not found in the test data.");
            }
        }

        //public async Task SwitchTabAsync(int tabIndex)
        //{
        //    var context = _browser.Contexts.FirstOrDefault();
        //    if (context == null)
        //    {
        //        throw new InvalidOperationException("No browser contexts available.");
        //    }

        //    var pages = context.Pages;
        //    if (tabIndex < 0 || tabIndex >= pages.Count)
        //    {
        //        throw new ArgumentOutOfRangeException(nameof(tabIndex), "Invalid tab index.");
        //    }

        //    _page = pages[tabIndex];
        //    await _page.BringToFrontAsync();
        //}
        public async Task SwitchTabAsync(int tabIndex)
        {
            Thread.Sleep(3000);
            // Get the browser context
            var context = _browser.Contexts.FirstOrDefault();
            if (context == null)
            {
                throw new InvalidOperationException("No browser contexts available.");
            }

            // Log the number of pages available
            var pages = context.Pages;
            Console.WriteLine($"[DEBUG] Total pages in context: {pages.Count}");

            // Check if the requested tab index is valid
            if (tabIndex < 0 || tabIndex >= pages.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(tabIndex), "Invalid tab index.");
            }

            // Log the switching action
            Console.WriteLine($"[DEBUG] Switching to tab index: {tabIndex}");

            // Switch to the desired tab
            _page = pages[tabIndex];
            await _page.BringToFrontAsync();
        }

        public async Task WaitForNetworkIdleAsync(IPage page, int timeout = 60000)
        {
            Console.WriteLine("[DEBUG] Waiting for network to be idle...");
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle, new PageWaitForLoadStateOptions { Timeout = timeout });
            Console.WriteLine("[DEBUG] Network is idle.");
        }

        public async Task WaitForOverlayToDisappear(string overlaySelector)
        {
            await _page.WaitForSelectorAsync(overlaySelector, new PageWaitForSelectorOptions
            {
                State = WaitForSelectorState.Hidden
            });
        }
        public static void AreEqual(object actual, object expected, string message = "")
        {
            if (!actual.Equals(expected))
            {
                throw new AssertionException($"Expected: {expected}. Actual: {actual}. {message}");
            }
        }

        public async Task AssertVerifyAsync(string actualGet, string expected)
        {
            try
            {
                if (actualGet.Equals(expected))
                {
                    AreEqual(actualGet, expected, "This we have expected: " + expected + " And We received: " + actualGet);
                    await AddScreenshot("passassertVerification");
                    _test.Log(Status.Pass, "Expected Message: " + expected + " Actual Message: " + actualGet);
                }
                else
                {
                    _test.Log(Status.Fail, "Expected Message: " + expected + " Actual Message: " + actualGet);
                    await AddScreenshot("failedassertVerification");
                }
            }
            catch (Exception e)
            {
                TestContext.WriteLine(e);
            }
        }

        public async Task<bool> WaitForMovementt(string selector)
        {
            var elementHandle = await _page.WaitForSelectorAsync(selector);
            if (elementHandle == null)
            {
                return false; // Element not found
            }

            var boundingBoxBefore = await elementHandle.BoundingBoxAsync();

            // Wait for a short delay
            await Task.Delay(500); // Adjust delay time as needed

            var boundingBoxAfter = await elementHandle.BoundingBoxAsync();

            if (boundingBoxBefore == null || boundingBoxAfter == null)
            {
                return false; // Element not visible or moved
            }

            // Compare position
            bool hasMoved = boundingBoxBefore.X != boundingBoxAfter.X || boundingBoxBefore.Y != boundingBoxAfter.Y;

            return hasMoved;
        }
    }
}













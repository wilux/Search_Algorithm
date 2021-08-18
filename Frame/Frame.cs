using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Selenium_Frame
{
    public static class Frame
    {

        public static string FrameActual(IWebDriver driver)
        {
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            string currentFrame = (string)jsExecutor.ExecuteScript("return self.name");

            return currentFrame;
        }

        public static int CantidadFrames(IWebDriver driver)
        {

            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            object frames = jsExecutor.ExecuteScript("return window.length");
            int cantidad = Convert.ToInt32(frames);
            if (cantidad == 0)
            {
                driver.SwitchTo().DefaultContent();
                IWebElement iframe = driver.FindElement(By.Id("0"));
                driver.SwitchTo().Frame(iframe);
                frames = jsExecutor.ExecuteScript("return window.length");
                cantidad = Convert.ToInt32(frames);

            }

            return cantidad;

        }

        public static bool BuscarFrame(IWebDriver driver, By locator)
        {

            int cantidad = CantidadFrames(driver);

            for (int i = 0; i < cantidad + 1; i++)
            {

                try
                {
                    driver.SwitchTo().DefaultContent();
                    driver.SwitchTo().Frame(4);
                    driver.SwitchTo().Frame("step" + i);
                    if (FindElementIfExists(driver, locator) != null)
                    {
                        return true;

                    }

                }
                catch { continue; }
            }


            return false;

        }

        }

        public static IWebElement FindElementIfExists(this IWebDriver driver, By by)
        {
            var elements = driver.FindElements(by);
            return (elements.Count >= 1) ? elements.First() : null;
        }

    }
}



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


        public static bool BuscarFrame(IWebDriver driver, By locator)
        {
            bool estado = false;

            if (FindElementIfExists(driver, locator) != null)
            {
                return true;
            }
            else
            {

                driver.SwitchTo().DefaultContent();
                string frameI = FrameActual(driver);
                int sizeInicial = driver.FindElements(By.TagName("iframe")).Count();

                for (int i = 0; i < sizeInicial; i++)
                {

                    driver.SwitchTo().Frame(i);
                    int sizeNuevo = driver.FindElements(By.TagName("iframe")).Count();
                    if (sizeNuevo != 0)
                    {
                        for (int j = 0; j < sizeNuevo; j++)
                        {
                            driver.SwitchTo().Frame(j);
                            try
                            {
                                string frameJ = FrameActual(driver);
                                driver.FindElement(locator);
                                estado = true;
                                break;
                            }
                            catch
                            {
                                driver.SwitchTo().ParentFrame();
                                continue;
                            }
                        }
                        if (estado == true) { break; } else { driver.SwitchTo().DefaultContent(); }
                    }
                    else { driver.SwitchTo().DefaultContent(); }
                }
            }

            return estado;
        }

    }


    }
}



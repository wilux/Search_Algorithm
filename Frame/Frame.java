package Tools;

import com.google.common.base.Stopwatch;
import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebElement;

import java.util.List;
import java.util.concurrent.TimeUnit;

public class Frame {

    WebDriver driver;


    public Frame(WebDriver driver) {

        this.driver = driver;

    }


    public String FrameActual() {
        JavascriptExecutor js = (JavascriptExecutor) driver;
        var currentFrame = js.executeScript ( "return self.name" );

        return currentFrame.toString ();
    }


    public Integer CantidadFrames() {


        //By finding all the web elements using iframe tag
        // List<IWebElement> iframeElements = driver.FindElements(By.TagName("iframe"));

        List<WebElement> iframeElements = driver.findElements ( By.tagName ( "iframe" ) );
        int cantidad = iframeElements.size ();

        //IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
        //string frames = jsExecutor.ExecuteScript("return window.length").ToString();
        //int cantidad = Convert.ToInt32(frames);

        if ( cantidad == 0 ) {
            driver.switchTo ().defaultContent ();
            WebElement iframe = driver.findElement ( By.id ( "0" ) );
            driver.switchTo ().frame ( iframe );
            //frames = jsExecutor.ExecuteScript("return window.length").ToString();
            //cantidad = Convert.ToInt32(frames);
            List<WebElement> iframeElements2 = driver.findElements ( By.tagName ( "iframe" ) );
            cantidad = iframeElements2.size ();

        }

        return cantidad;

    }


    public WebElement FindElementIfExists(By by) {
        WebElement element = null;
        final Stopwatch stopwatch = Stopwatch.createStarted ();
        do {

            try {
                List<WebElement> elements = driver.findElements ( by );
                element = (elements.size () >= 1) ? elements.get ( 0 ) : null;
                break;
            } catch (Exception e) {
                continue;
            }

        } while (stopwatch.elapsed ( TimeUnit.SECONDS ) < 5);

        return element;
    }


    public Boolean BuscarFrame(By locator) {
        Boolean estado = false;


        if ( FindElementIfExists ( locator ) != null ) {
            estado = true;
        }
        else {

            try {
                driver.switchTo ().defaultContent ();
            } catch (Exception e) {
                //   System.out.println(e);

            }

            String frameI = FrameActual ();
            int sizeInicial = driver.findElements ( By.tagName ( "iframe" ) ).size ();
            for (int i = 0; i < sizeInicial; i++) {
                driver.switchTo ().frame ( i );
                int sizeNuevo = driver.findElements ( By.tagName ( "iframe" ) ).size ();
                if ( sizeNuevo != 0 ) {
                    for (int j = 0; j < sizeNuevo; j++) {
                        driver.switchTo ().frame ( j );
                        try {
                            String frameJ = FrameActual ();
                            driver.findElement ( locator );
                            estado = true;
                            break;
                        } catch (Exception e) {
                            driver.switchTo ().parentFrame ();
                            continue;
                        }
                    }
                    if ( estado == true ) {
                        break;
                    }
                    else {
                        driver.switchTo ().defaultContent ();
                    }
                }
                else {
                    driver.switchTo ().defaultContent ();
                }
            }
        }

        return estado;
    }


}

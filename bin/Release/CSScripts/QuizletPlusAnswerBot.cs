//css_reference {Directory.GetCurrentDirectory()}\Dlls\SeleniumExtras.WaitHelpers.dll;

string birthDayMonth = "birth_month", birthDay = "birth_day", birthDayYear = "birth_year",
    emailElementName = "email", usernameElementName = "username",
    passwordElementName = "password1", signUpButtonClassName = "UILoadingButton";

public void Main()
{
	try
	{
		ChromeDriverWait.Until(webDriver => webDriver.FindElement(By.Name(birthDayMonth)).Displayed);
		
		var birthDayMonthElement = ChromeDriver.FindElement(By.Name(birthDayMonth));
		var selectBirthDayMonthElement = new SelectElement(birthDayMonthElement);
		selectBirthDayMonthElement.SelectByText("July");
	
		var birthDayElement = ChromeDriver.FindElement(By.Name(birthDay));
		var selectBirthDayElement = new SelectElement(birthDayElement);
		selectBirthDayElement.SelectByText("3");
	
		var birthDayYearElement = ChromeDriver.FindElement(By.Name(birthDayYear));
		var selectBirthDayYearElement = new SelectElement(birthDayYearElement);
		selectBirthDayYearElement.SelectByText("2000");
	
		var emailNameElement = ChromeDriver.FindElement(By.Name(emailElementName));
		emailNameElement.SendKeys($"{RandomName(15)}@gmail.com");
	
		var usernameElement = ChromeDriver.FindElement(By.Name(usernameElementName));
		usernameElement.Clear();
		usernameElement.SendKeys(RandomName(5));
	
		var passwordElement = ChromeDriver.FindElement(By.Name(passwordElementName));
		passwordElement.SendKeys(RandomName());
	
		Thread.Sleep(800);
		ChromeDriverWait.Until(webDriver => webDriver.FindElement(By.ClassName(signUpButtonClassName)).Enabled);
	
		var signUpButtonElement = ChromeDriver.FindElement(By.ClassName(signUpButtonClassName));
		signUpButtonElement.Click();
	}
	catch (Exception e)
	{
		Console.WriteLine(e);
	}
}

public static string RandomName(int length = 25, bool lowerCase = false)
{
    var builder = new StringBuilder(length);

    var offset = lowerCase ? 'a' : 'A';
    const int lettersOffset = 26; // A...Z or a..z: length = 26  

    for (var i = 0; i < length; i++)
    {
        var @char = (char)new Random().Next(offset, offset + lettersOffset);
        builder.Append(@char);
    }

    return lowerCase ? builder.ToString().ToLower() : builder.ToString();
}
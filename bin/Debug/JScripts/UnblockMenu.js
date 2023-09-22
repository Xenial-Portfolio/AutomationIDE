var cachedDocumentTitle = document.title;
var cachedDocument = document.body.innerHTML;
var cachedBackgroundColor = document.body.style.backgroundColor;
var cachedBackgroundImage = document.body.style.backgroundImage;

document.addEventListener('keydown', function(event) 
{
    if(event.keyCode == 37) 
	{
		document.body.style.backgroundImage = 'none';
		document.body.style.backgroundColor = "#181a1b";
		document.title = "Granville CSD Admin Panel!";
	
		var h1s = document.getElementsByTagName("h1");
		for (var i = 0; i < h1s.length; i++) 
		{
			var h1 = h1s[i];
			if (!h1.id) h1.innerHTML = "Granville CSD Admin Panel";
		}
		
		var userNameLoginData = document.getElementById("txtUserName");
		var passwordLoginData = document.getElementById("txtPassword");
		var loginButton = document.getElementById("btnLogin");
		
		loginButton.addEventListener("click", function () 
		{ 
			if (userNameLoginData.value != "Admin" && passwordLoginData.value != "password")
			{
				alert('Login Failed Try Again!');
				return;
			}
			
			alert('Login Successful');

			document.body.innerHTML = '<head> <button id="LoggedIn" type="button" onclick="myFunc();">Grant Admin Access!</button> </head>';
			var newScript = document.createElement("script");
			newScript.type = "text/javascript";
			newScript.innerHTML = 'function myFunc() { alert(\'Unblocking All Items On Your PC!\'); location.href = \'https://jrsr.granvillecsd.org/\'; }';
			document.getElementsByTagName('body')[0].appendChild(newScript);
		}); 
	}
    else if(event.keyCode == 39) 
	{
		document.title = cachedDocumentTitle;
        document.body.innerHTML = cachedDocument;
		document.body.style.backgroundImage = cachedBackgroundImage;
		document.body.style.backgroundColor = cachedBackgroundColor;
    }
});
# WpfLocalization
Localization/Language-Selection for WPF

This is a simple tool for WPF localization.
- only one Resource.resx file containing all different languages
- change language on runtime
- binding in xaml and code behind possible
- generate strings with parameters
- tool to edit and maintain the different languages

How To Localization  
 
1. Add the reference to the assembly 'Localization' to your project   
2. Add Resources.resx to your project properties   
3. Create a method for the application startup in App.xaml.cs, set this method in App.xaml and initialize the Localization Dictionary

	App.xaml:
	Startup="Application_Startup"
	
	App.xaml.cs
	private void Application_Startup(object sender, StartupEventArgs e)
	{
		var assemblies = new List<Assembly>();
		assemblies.Add(typeof(App).Assembly);
		/* Add all assemblies you want to use localization */ 
		LocDict.Instance.Initialize("Translations.xml", assemblies);

		var main = new MainWindow();
		main.Show();
	}
	   
4. Add for every translatable string a key(name) in the Resources.resx and leave Value empty    
5. Start the application - the localization file will be created    
6. Copy the created file from Debug/Release to your main project and add it to your project. Set the file to "Content" and "Copy always"     
7. Now you can add with the Localization Editor a different language to your file (e.g. 'de-DE' for German) and translate all keys");

Usage
- Binding in *.xaml
	1. Add reference
		xmlns:loc="clr-namespace:Localization;assembly=Localization"
	2. Set binding to key that is registered in Resources.resx
		Header="{loc:Loc UiOpen}"

- Code behind *.cs
	1. Add reference
		using Localization;
	2. Key without parameters
		string translatedKey = LocDict.Instance.Get(nameof(Properties.Resources.MsgSaveFile));
	3. Key with parameters
		string translatedKey = LocDict.Instance.Get(nameof(Properties.Resources.MsgSaveFile), nameof(Properties.Resources.TranslateableParamter), "ParameterWithoutTranslation", "4");
		

Change language
- English/Default	
    LocDict.Instance.CurrentCulture = CultureInfo.InvariantCulture;
	
-Other language
	LocDict.Instance.CurrentCulture = new CultureInfo("de-DE");


Note
If you delete a unnecessary key from Resources.resx, the key will be not deleted from Translations.xml
You can delete the key from Editor through a right click on the key
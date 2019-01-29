# Multilanguage Chat

This sample shows how to use Azure SignalR, Microsoft Translator and Speech Services to build a real-time voice translation system for ASP.NET Core, Windows 10 and Xamarin.

**Getting started**

To use these applications, you must obtain the required Keys and Connection Strings from [Azure Portal](https://portal.azure.com) and insert the correct values in the configuration files, as describe later.

**Create the required services on the Azure Portal**

- Create a new [Azure SignalR Service](https://portal.azure.com/#create/Microsoft.SignalRGalleryPackage). For best perfomance, choose a Location near you

- Create a new [Translator Text Service](https://portal.azure.com/#create/Microsoft.CognitiveServicesTextTranslation)

- Create a new [Speech Service](https://portal.azure.com/#create/Microsoft.CognitiveServicesSpeechServices). For best perfomance, choose a Location near you

**Configure the Web Backend**

Go to the [appsettings.json](https://github.com/marcominerva/MultilanguageChat/blob/master/Web/MultilanguageChat/appsettings.json) file and write the required information:

- In the **Azure:SignalR:ConnectionString** property, specify the Connection String you can find in the **Keys** blade of the Azure SignalR Service

- In the **AppSettings:TranslatorSubscriptionKey** property, speficy the key of the Translator Text Service (you can use either *Key 1* or *Key 2* available in the **Keys** blade of the Service)

- Publish the Web App on an Azure App Service.

**Configure the UWP app**

Go to the [Constants.cs](https://github.com/marcominerva/MultilanguageChat/blob/master/Windows/MultilanguageChat/Common/Constants.cs) file and write the required information:

- In the **ServerUrl** field, specify the Url of the Web App on Azure with the */chat* suffix, for example:
    > https://myappsite.azurewebsites.net/chat

- In the **TranslatorSubscriptionKey** field, speficy the key of the Translator Text Service (you can use either *Key 1* or *Key 2* available in the **Keys** blade of the Service)

- In the **SpeechRegion** field, specify the value corresponding to the Azure Region in which you have created the Speech Service, using the **Speeck SDK Parameters** that you can find at [Speech Service supported regions](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/regions#speech-recognition-and-translation)

- In the **SpeechSubscriptionKey** field, speficy the key of the Speech Service (you can use either *Key 1* or *Key 2* available in the **Keys** blade of the Service)

**Configure the Xamarin app**

Go to the [Constants.cs](https://github.com/marcominerva/MultilanguageChat/blob/master/App/MultilanguageChat/Common/Constants.cs) file and write the required information (the same as above).

**Contribute**

The project is continuously evolving. We welcome contributions. Feel free to file issues and pull requests on the repo and we'll address them as we can.

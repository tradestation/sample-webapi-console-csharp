# sample-webapi-console-csharp

This sample application uses C# with .NET 4.0 to authenticate with the TradeStation API via an OAuth 2 Authorizatin Code Grant Type. The user will be directed to TradeStation's login page to capture credentails. After a successful login, an auth code is return and is then exchanged for an access token which will be used for subsequent WebAPI calls.

## Configuration
Modify the following fields in the app.config with your appropriate values:

    <applicationSettings>
        <SymbolSuggestDemo.Properties.Settings>
            <setting name="APIKey" serializeAs="String">
                <value>your key goes here</value>
            </setting>
            <setting name="APISecret" serializeAs="String">
                <value>your secret goes here</value>
            </setting>
            <setting name="RedirectUri" serializeAs="String">
                <value>your redirect URI goes here</value> // Example: http://www.tradestation.com
            </setting>
            <setting name="Environment" serializeAs="String">
                <value>Can be "SIM" or "LIVE"</value>
            </setting>
        </SymbolSuggestDemo.Properties.Settings>
    </applicationSettings>

## Build Instructions
* Download and Extract the zip or clone this repo
* Open Visual Studio
* Build and Run

## Troubleshooting
If there are any problems, open an [issue](https://github.com/tradestation/sample-webapi-console-csharp/issues) and we'll take a look! You can also give us feedback by e-mailing webapi@tradestation.com.

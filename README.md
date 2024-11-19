Steps zum erfolgreichem starten des Programms:
- Erstelle ein Konto bei Bexio https://www.bexio.com
- Gehe unter Controllers/AuftragController.cs
undersetzt die Zeilen mit deinen Werten
        private const string XmlFilePath = @"DEIN-XML-FILE-PATH";
        private const string ClientId = "DEINE-CLIENT-ID";
        private const string ClientSecret = "DEIN-CLIENT-SECRET";
        private string BexioApiAccessToken = "DEIN-BEXIO-API-KEY";
        private const string BexioApiUrl = "DIENE-BEXIO-API-URL";
        private const string RedirectUri = "DEIN-REDIRECT-URL";
        private const string ContactsEndpoint = "DEIN-CONTACT-ENDPOINT";
        private const string AuthorizationEndpoint = "DEIN-AUTHORIZATION-ENDPOINT";
        private const string TokenEndpoint = "DEIN-TOKEN-ENDPOINT";
        private const string JWKEndpoint = "DEIN-JWK-ENDPOINT";

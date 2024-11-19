using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace Auftrag.Modul.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuftragController : ControllerBase
    {
        private readonly HttpClient _httpClient;


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


        public AuftragController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }




        //daniel aufgabe
        [HttpPost("bessererPost")]
        public async Task<IActionResult> CreateAuftrag(
        [FromForm] string title,
        [FromForm] int contact_id,
        [FromForm] int user_id,
        [FromForm] int language_id,
        [FromForm] int bank_account_id,
        [FromForm] int currency_id,
        [FromForm] int payment_type_id,
        [FromForm] int mwst_type,
        [FromForm] string header,
        [FromForm] string footer,
        [FromForm] DateTime is_valid_from,
        [FromForm] int project_id)
        {
            if (string.IsNullOrEmpty(BexioApiAccessToken))
            {
                return BadRequest("Kein gültiges Access Token gefunden.");
            }

            // Erstelle das Auftrag-Objekt
            var postAuftrag = new Auftrag
            {
                title = title,
                contact_id = contact_id,
                user_id = user_id,
                language_id = language_id,
                bank_account_id = bank_account_id,
                currency_id = currency_id,
                payment_type_id = payment_type_id,
                mwst_type = mwst_type,
                header = header,
                footer = footer,
                is_valid_from = is_valid_from.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                project_id = project_id
            };

            var requestUrl = "https://api.bexio.com/2.0/kb_order"; // Ersetze dies durch die korrekte URL für die API

            // Serialisieren des Auftrags in JSON
            var jsonContent = JsonSerializer.Serialize(postAuftrag, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {BexioApiAccessToken}");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            // Senden der POST-Anfrage
            var response = await _httpClient.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Fehler beim Erstellen des Auftrags: {errorDetails}");
            }

            return Ok("Auftrag wurde erfolgreich erstellt");
        }


        //erstelle einen Auftrag (ohne xml)
        //[HttpPost("Post")]
        //public async Task<IActionResult> CreateAuftrag([FromBody] Auftrag auftrag)
        //{
        //    if (string.IsNullOrEmpty(BexioApiAccessToken))
        //    {
        //        return BadRequest("Kein gültiges Access Token gefunden.");
        //    }

        //    // Remove Id property for POST requests
        //    var postAuftrag = new Auftrag
        //    {
        //        title = auftrag.title,
        //        contact_id = auftrag.contact_id,
        //        user_id = auftrag.user_id,
        //        language_id = auftrag.language_id,
        //        bank_account_id = auftrag.bank_account_id,
        //        currency_id = auftrag.currency_id,
        //        payment_type_id = auftrag.payment_type_id,
        //        mwst_type = auftrag.mwst_type,
        //        header = auftrag.header,
        //        footer = auftrag.footer,
        //        is_valid_from = auftrag.is_valid_from,
        //        project_id = auftrag.project_id
        //    };

        //    var requestUrl = "https://api.bexio.com/2.0/kb_order"; // Ersetze dies durch die korrekte URL für die API

        //    // Serialisieren des Auftrags in JSON
        //    var jsonContent = JsonSerializer.Serialize(postAuftrag, new JsonSerializerOptions
        //    {
        //        PropertyNamingPolicy = JsonNamingPolicy.CamelCase // PropertyNamingPolicy auf camelCase setzen
        //    });

        //    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //    _httpClient.DefaultRequestHeaders.Clear();
        //    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {BexioApiAccessToken}");
        //    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

        //    // Senden der POST-Anfrage
        //    var response = await _httpClient.PostAsync(requestUrl, content);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        var errorDetails = await response.Content.ReadAsStringAsync();
        //        return StatusCode((int)response.StatusCode, $"Fehler beim Erstellen des Auftrags: {errorDetails}");
        //    }

        //    var responseContent = await response.Content.ReadAsStringAsync();
        //    return Ok("Auftrag wurde erfolgreich erstellt");
        //}




        //Gibt alle Aufträge an
        [HttpGet("Get")]
        public async Task<IActionResult> GetAuftraege()
        {
            if (string.IsNullOrEmpty(BexioApiAccessToken))
            {
                return BadRequest("Kein gültiges Access Token gefunden.");
            }

            // Anfrage an die Bexio API, um die Aufträge abzurufen
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {BexioApiAccessToken}");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var auftraegeEndpoint = "https://api.bexio.com/2.0/kb_order";
            var response = await _httpClient.GetAsync(auftraegeEndpoint);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Fehler beim Abrufen der Aufträge: {errorDetails}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserialisierung der JSON-Antwort in eine Liste von Aufträgen
            var auftraege = JsonSerializer.Deserialize<List<Auftrag>>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            });

            // Rückgabe der Aufträge im JSON-Format
            return Ok(auftraege);
        }


        //Auftrag Löschen
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAuftrag(string id)
        {
            if (string.IsNullOrEmpty(BexioApiAccessToken))
            {
                return BadRequest("Kein gültiges Access Token gefunden.");
            }

            var requestUrl = $"https://api.bexio.com/2.0/kb_order/{id}"; // Ersetze dies durch die korrekte URL für die API

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {BexioApiAccessToken}");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            // Senden der DELETE-Anfrage
            var response = await _httpClient.DeleteAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Fehler beim Löschen des Auftrags: {errorDetails}");
            }

            return Ok("Erfolgreich gelöst"); // Erfolgreiches Löschen ohne Rückgabewert
        }



        //Get Contacts und filtere nach Namen und Vorname
        [HttpGet("contacts")]
        public async Task<IActionResult> GetBexioContacts([FromQuery] string name_1 = null, [FromQuery] string name_2 = null)
        {
            if (string.IsNullOrEmpty(BexioApiAccessToken))
            {
                return BadRequest("Kein gültiges Access Token gefunden.");
            }

            // Anfrage an die Bexio API, um die Kontaktliste zu erhalten
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {BexioApiAccessToken}");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await _httpClient.GetAsync(ContactsEndpoint);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Fehler beim Abrufen der Kontakte: {errorDetails}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var contacts = JsonSerializer.Deserialize<List<BexioContact>>(jsonResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            // Filtere die Kontakte nach name_1 oder name_2, falls angegeben
            var filteredContacts = contacts.Where(c =>
                (string.IsNullOrEmpty(name_1) || (!string.IsNullOrEmpty(c.name_1) && c.name_1.StartsWith(name_1, StringComparison.OrdinalIgnoreCase))) &&
                (string.IsNullOrEmpty(name_2) || (!string.IsNullOrEmpty(c.name_2) && c.name_2.StartsWith(name_2, StringComparison.OrdinalIgnoreCase)))
            ).ToList();

            return Ok(filteredContacts);
        }




        //erstellt einen Auftrag (mit xml)
        [HttpPost("xml")]
        public async Task<IActionResult> UploadAndSendOrder()
        {
            if (!System.IO.File.Exists(XmlFilePath))
            {
                return NotFound("Die XML-Datei wurde nicht gefunden.");
            }
            try
            {
                // 1. Read the XML file content
                var xmlContent = System.IO.File.ReadAllText(XmlFilePath);

                // 2. Convert XML to JSON
                var jsonContent = ConvertXmlToJson(xmlContent);

                // 3. Send JSON to the Bexio API
                if (string.IsNullOrEmpty(BexioApiAccessToken))
                {
                    return BadRequest("Kein gültiges Access Token gefunden.");
                }

                var requestUrl = "https://api.bexio.com/2.0/kb_order";

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {BexioApiAccessToken}");
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(requestUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return Ok("Auftrag erfolgreich erstellt.");
                }

                else
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"Fehler beim Erstellen des Auftrags: {errorDetails}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (or use a logging framework)
                return StatusCode(500, $"Ein unerwarteter Fehler ist aufgetreten: {ex.Message}");
            }
        }




        //------Funktionen------

        private string ConvertXmlToJson(string xmlContent)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlContent);

            var json = XmlToJson(xmlDocument.DocumentElement);
            return JsonSerializer.Serialize(json, new JsonSerializerOptions { WriteIndented = true });
        }

        private static object XmlToJson(XmlNode node)
        {
            if (node is XmlElement element)
            {
                var result = new Dictionary<string, object>();

                foreach (XmlNode child in element.ChildNodes)
                {
                    if (child is XmlElement childElement)
                    {
                        if (childElement.HasChildNodes)
                        {
                            var childResult = XmlToJson(childElement);
                            if (result.ContainsKey(childElement.Name))
                            {
                                var existingValue = result[childElement.Name] as List<object>;
                                if (existingValue == null)
                                {
                                    existingValue = new List<object> { childResult };
                                    result[childElement.Name] = existingValue;
                                }
                                else
                                {
                                    existingValue.Add(childResult);
                                }
                            }
                            else
                            {
                                result[childElement.Name] = childResult;
                            }
                        }
                        else
                        {
                            result[childElement.Name] = childElement.InnerText;
                        }
                    }
                    else if (child is XmlText text)
                    {
                        return text.Value;
                    }
                }

                return result;
            }

            return null;
        }
    }
}
﻿using EHVAG.MusiGModel;
using Newtonsoft.Json.Linq;
using StatsHelix.Charizard;
using System.Collections.Generic;
using static StatsHelix.Charizard.HttpResponse;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace EHVAG.MusiGServer.Controller
{
    [Controller]
    public class oAuth2CallbackController
    {
        // TODO: Check who is calling this endpoint. Cookies?
        // TODO: What if we "error" as parameter. This won't work.
        public async Task<HttpResponse> YouTubeResponse(string code = null, string error = null)
        {
            // Check for bad requests
            if (System.String.IsNullOrEmpty(code) && System.String.IsNullOrEmpty(error))
                return Redirect(StaticPages.BadRequest);

            // User gave us no access
            if (!string.IsNullOrEmpty(error))
                return String("Fells bad man");

            dynamic responseJson;
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "code", code},
                    { "client_id", Program.googleClientSecrets.web.client_id.ToString()},
                    { "client_secret", Program.googleClientSecrets.web.client_secret.ToString() },
                    { "redirect_uri", Program.googleClientSecrets.web.redirect_uris[0].ToString() },
                    { "grant_type", Program.googleClientSecrets.web.grant_type.ToString() }
                };

                // Exchange AuthCode for AccessToken and RefreshToken
                var response = await client.PostAsync(Program.googleClientSecrets.web.token_uri.ToString(), new FormUrlEncodedContent(values));
                responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());
            }

            // Handle response
            if (responseJson.error == null)
            {
                // "Design problem" where to get the Channel entity and User entity for the method call. 
                // We need two database lookups. Or one if we use a join.
                // Do i have better design options? I want to validate if the user already has the channel authenticated.
                // This could be the case if you call the controller manually.
                // The call should be the same for all channels with OAuth. So i created a method. DRY.
                if (!OAuth2TokenExists("YouTube", 123456))
                {
                    var token = new OAuth2Token();
                    using (var context = new MusiGDBContext())
                    {
                        token.Channel = await context.Channel.Where(c => c.Name == "YouTube").FirstOrDefaultAsync();

                        // TODO: We don't know the user yet. Add it later.
                        token.UserId = 1;
                        token.AccessToken = responseJson.access_token;
                        token.TokenExpiresAt = DateTimeOffset.UtcNow.AddSeconds(Convert.ToDouble(responseJson.expires_in));
                        token.TokenType = responseJson.token_type;
                        token.RefreshToken = responseJson.refresh_token;

                        context.OAuth2Token.Add(token);
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    // User has channel already authenticated.
                    // Maybe present an usefull help page?
                    Redirect(StaticPages.AlreadyAuthenticated);
                }
                // TODO: What do we do if this succeeds? Redirect? Where?
            }
            // If we get here; Something went wrong. Present answer from the server
            return Json(responseJson, HttpStatus.InternalServerError);
        }

        private async Task<bool> OAuth2TokenExists(Channel channel, User userId)
        {
            using (var context = new MusiGDBContext())
            {
                return await context.OAuth2Token.AnyAsync(c => c.Channel == channel && c.User == userId);
            }
        }
    }
}

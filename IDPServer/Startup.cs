using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace IDPServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddIdentityServer(option => {
                option.Caching.ClientStoreExpiration = new TimeSpan(0, 5, 0);
                

            })
                .AddDeveloperSigningCredential()
                   .AddTestUsers(new List<IdentityServer4.Test.TestUser>
                   {
                       new IdentityServer4.Test.TestUser
                       {
                            SubjectId="1",
                            Username="akbar",
                            Password="12341234",
                            Claims=new List<Claim>
                            {
                                new Claim(ClaimTypes.Email,"a@gmail.com"),
                                new Claim(ClaimTypes.Name,"akbar"),
                                new Claim(ClaimTypes.MobilePhone,"09129224376"),
                                new Claim("family","rezaeyan"),
                            }

                       }
                    }).
                   AddInMemoryIdentityResources(new List<IdentityResource>
                   {
                       new IdentityResources.OpenId(),
                       new IdentityResources.Email(),
                       new IdentityResources.Profile(),
                       new IdentityResources.Phone(),
                   })
                   .AddInMemoryClients(new List<Client>
                   {
                       new Client
                       {
                            ClientId="154",
                            ClientSecrets=new List<Secret>
                            {
                                new Secret("123456".Sha256())
                            },
                            AllowedGrantTypes=GrantTypes.Implicit,
                            RedirectUris={"https://localhost:44383/signin-oidc"},
                            PostLogoutRedirectUris={"https://localhost:44383/signout-callback-oidc"},
                            AllowedScopes=new List<string>
                            {
                                StandardScopes.OpenId,
                                StandardScopes.Email,
                                StandardScopes.Profile,
                                StandardScopes.Phone

                            },
                            RequireConsent=false

                       },
                        new Client
                       {
                            ClientId="15",
                            ClientSecrets=new List<Secret>
                            {
                                new Secret("123456".Sha256())
                            },
                            AllowedGrantTypes=GrantTypes.Implicit,
                            RedirectUris={"https://localhost:7247/signin-oidc"},
                            PostLogoutRedirectUris={"https://localhost:7247/signout-callback-oidc"},
                            AllowedScopes=new List<string>
                            {
                                StandardScopes.OpenId,
                                StandardScopes.Email,
                                StandardScopes.Profile,
                                StandardScopes.Phone

                            },
                            RequireConsent=false

                       }
                   }).
                   AddInMemoryApiResources(new List<ApiResource>
                   {

                   });

                   
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

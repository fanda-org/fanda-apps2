using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanda.Authentication.Repository;
using Fanda.Authentication.Repository.Dto;
using Fanda.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fanda.Authentication.Service
{
    public class SeedDefault
    {
        //private readonly AppSettings _settings;
        private readonly ILogger<SeedDefault> _logger;
        private readonly IServiceProvider _provider;

        public SeedDefault(IServiceProvider provider /*, IOptions<AppSettings> options*/)
        {
            _provider = provider;
            //_settings = options.Value;
            _logger = _provider.GetRequiredService<ILogger<SeedDefault>>();
        }

        public async Task CreateTenantAsync()
        {
            try
            {
                var repository = _provider.GetRequiredService<ITenantRepository>();

                // create a super user who could maintain entire web app
                var tenantInput = new TenantDto
                {
                    Code = "FANDA",
                    Name = "Fanda",
                    Description = "Fanda tenant",
                    Active = true,
                    OrgCount = 999999
                };
                if (!await repository.AnyAsync(t => t.Code == tenantInput.Code)
                ) //ExistsAsync(new KeyData { Field = KeyField.Code, Value = tenantInput.Code }))
                {
                    var tenantOutput = await repository.CreateAsync(tenantInput);
                    await CreateUsersAsync(tenantOutput);
                    await CreateRolesAsync(tenantOutput);

                    // await repository.MapOrgAsync(user.Id, org.Id);
                    // await repository.MapRoleAsync(user.Id, "SuperAdmin", org.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async Task CreateUsersAsync(TenantDto tenant)
        {
            try
            {
                var repository = _provider.GetRequiredService<IUserRepository>();

                // creating a super user who could maintain the web app
                var superAdmin = new UserDto
                {
                    UserName = "FandaAdmin", Email = "fandaadmin@fandatech.net", Password = "Welcome@123", Active = true
                    //TenantId = tenant.Id
                };
                if (!await repository.AnyAsync(Guid.Empty, u => u.UserName == superAdmin.UserName))
                    //new UserKeyData { TenantId = tenant.Id, Field = KeyField.Name, Value = superAdmin.UserName })
                {
                    var user = await repository.CreateAsync(tenant.Id, superAdmin);
                    // await repository.MapOrgAsync(user.Id, org.Id);
                    // await repository.MapRoleAsync(user.Id, "SuperAdmin", org.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async Task CreateRolesAsync(TenantDto tenant)
        {
            try
            {
                var repository = _provider.GetRequiredService<IRoleRepository>();

                //adding customs roles
                string[] rolesArray =
                {
                    //"SuperAdmin:Super Administrators have complete and unrestricted access to the application",
                    "Admin:Administrators have complete and unrestricted access to the organization"
                    //"Manager:Managers are possess limited administrative powers",
                    //"SuperUser:Super users are have additional access than users",
                    //"User:Users are prevented from making accidental or intentional system-wide changes and can run most"
                };

                foreach (string roleElement in rolesArray)
                {
                    string roleName = roleElement.Split(':')[0];
                    string description = roleElement.Split(':')[1];
                    string roleCode = roleName.ToUpper();
                    // creating the roles and seeding them to the database
                    if (!await repository.AnyAsync(tenant.Id, r => r.Code == roleCode)
                    ) //ExistsAsync(new TenantKeyData { TenantId = tenant.Id, Field = KeyField.Code, Value = roleCode }))
                    {
                        var roleInput = new RoleDto
                        {
                            Code = roleCode,
                            Name = roleName,
                            Description = description,
                            Active = true,
                            Privileges = await GetRolePrivileges()
                        };
                        var roleOutput = await repository.CreateAsync(tenant.Id, roleInput);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async Task<List<RolePrivilegeDto>> GetRolePrivileges()
        {
            var appRepo = _provider.GetRequiredService<IApplicationRepository>();
            var apps = await appRepo
                .FindAsync(app => app.Code == "FANDA"); //(new KeyData { Field = KeyField.Code, Value = "FANDA" });
            var allResource = apps?.FirstOrDefault().AppResources.FirstOrDefault(ar => ar.Code == "*");
            return new List<RolePrivilegeDto>
            {
                new RolePrivilegeDto
                {
                    AppResourceId = allResource.Id,
                    Create = true,
                    Delete = true,
                    Export = true,
                    Import = true,
                    Print = true,
                    Read = true,
                    Update = true
                }
            };
        }

        #region Fanda App

        public async Task CreateFandaAppAsync()
        {
            try
            {
                string appName = "Fanda";
                string appCode = appName.ToUpper();

                var repository = _provider.GetRequiredService<IApplicationRepository>();

                if (!await repository.AnyAsync(app => app.Code == appCode)
                ) //new KeyData { Field = KeyField.Code, Value = appCode }))
                {
                    var appInput = new ApplicationDto
                    {
                        Code = appCode,
                        Name = appName,
                        Description = "Finance and accounting system with inventory management",
                        Active = true,
                        Edition = "Standard",
                        Version = "1.0.0",
                        AppResources = GetAppResources()
                    };
                    var appOutput = await repository.CreateAsync(appInput);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private static List<AppResourceDto> GetAppResources()
        {
            return new List<AppResourceDto>
            {
                new AppResourceDto
                {
                    Code = "*",
                    Name = "All",
                    Description = "All resources",
                    Active = true,
                    ResourceType = ResourceType.Master | ResourceType.Transaction
                                                       | ResourceType.Configuration | ResourceType.Report,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "ROLES",
                    Name = "Roles",
                    Description = "Role master",
                    Active = true,
                    ResourceType = ResourceType.Master,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "USERS",
                    Name = "Users",
                    Description = "User master",
                    Active = true,
                    ResourceType = ResourceType.Master,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "LEDGERS",
                    Name = "Ledgers",
                    Description = "Ledger master",
                    Active = true,
                    ResourceType = ResourceType.Master,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "LEDGER_GROUPS",
                    Name = "Ledger Groups",
                    Description = "Ledger group master",
                    Active = true,
                    ResourceType = ResourceType.Master,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "ACCT_YEARS",
                    Name = "Accounting Years",
                    Description = "Accounting year master",
                    Active = true,
                    ResourceType = ResourceType.Master,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "PUR_INV",
                    Name = "Purchase Invoices",
                    Description = "Purchase invoices",
                    Active = true,
                    ResourceType = ResourceType.Transaction,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "SAL_INV",
                    Name = "Sales Invoices",
                    Description = "Sales invoices",
                    Active = true,
                    ResourceType = ResourceType.Transaction,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "PURRET_INV",
                    Name = "Purchase Return Invoices",
                    Description = "Purchase return invoices",
                    Active = true,
                    ResourceType = ResourceType.Transaction,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "SALRET_INV",
                    Name = "Sales Return Invoices",
                    Description = "Sales return invoices",
                    Active = true,
                    ResourceType = ResourceType.Transaction,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "STOCK",
                    Name = "Stock",
                    Description = "Stock",
                    Active = true,
                    ResourceType = ResourceType.Transaction,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "ORGS",
                    Name = "Organizations",
                    Description = "Organization master",
                    Active = true,
                    ResourceType = ResourceType.Master,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "CUSTOMERS",
                    Name = "Customers",
                    Description = "Customer master",
                    Active = true,
                    ResourceType = ResourceType.Master,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "SUPPLIERS",
                    Name = "Suppliers",
                    Description = "Supplier master",
                    Active = true,
                    ResourceType = ResourceType.Master,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "BANKS",
                    Name = "Banks",
                    Description = "Bank master",
                    Active = true,
                    ResourceType = ResourceType.Master,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "BUYERS",
                    Name = "Buyers",
                    Description = "Buyer master",
                    Active = true,
                    ResourceType = ResourceType.Master,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "PARTY_CAT",
                    Name = "Party Categories",
                    Description = "Party category master",
                    Active = true,
                    ResourceType = ResourceType.Master,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "PRODUCTS",
                    Name = "Products",
                    Description = "Product master",
                    Active = true,
                    ResourceType = ResourceType.Master,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "PROD_BRANDS",
                    Name = "Product Brands",
                    Description = "Product brand master",
                    Active = true,
                    ResourceType = ResourceType.Master,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "PROD_SEGS",
                    Name = "Product Segments",
                    Description = "Product segment master",
                    Active = true,
                    ResourceType = ResourceType.Master,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "PROD_VARS",
                    Name = "Product Varieties",
                    Description = "Product variety master",
                    Active = true,
                    ResourceType = ResourceType.Master,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "PROD_PRICINGS",
                    Name = "Product Pricings",
                    Description = "Product pricing master",
                    Active = true,
                    ResourceType = ResourceType.Master,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                },
                new AppResourceDto
                {
                    Code = "UNITS",
                    Name = "Units",
                    Description = "Unit master",
                    Active = true,
                    ResourceType = ResourceType.Master,
                    Creatable = true,
                    Readable = true,
                    Updatable = true,
                    Deletable = true,
                    Importable = true,
                    Exportable = true,
                    Printable = true
                }
            };
        }

        #endregion Fanda App
    }
}
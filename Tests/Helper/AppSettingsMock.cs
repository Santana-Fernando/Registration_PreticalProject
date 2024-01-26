using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Helper
{
    public class AppSettingsMock
    {
        public Mock<IConfiguration> configurationMockStub()
        {
            const string jwtKey = "ChaveSuperSecreta123";
            const string jwtIssuer = "FERNANDO";
            const string jwtAudience = "AplicacaoWebAPI";
            const int jwtExpirationInMinutes = 30;

            var configurationMock = new Mock<IConfiguration>();

            configurationMock.Setup(x => x["Jwt:Key"]).Returns(jwtKey);
            configurationMock.Setup(x => x["Jwt:Issuer"]).Returns(jwtIssuer);
            configurationMock.Setup(x => x["Jwt:Audience"]).Returns(jwtAudience);
            configurationMock.Setup(x => x["Jwt:ExpirationInMinutes"]).Returns(jwtExpirationInMinutes.ToString());

            return configurationMock;
        }

        public DbContextOptions<ApplicationDbContext> OptionsDatabaseStub()
        {
            const string defaultConnectionString = "Data Source=DESKTOP-F9M2LAN\\SQLEXPRESS;User ID=sa;Password=Fern@nd01331;Database=PeopleBasePraticle;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(defaultConnectionString)
                .Options;

            return options;
        }
    }
}

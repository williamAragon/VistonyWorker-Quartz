using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.MigracionCliente.Interfaces;
using Vistony.Worker.Domain.MigracionCliente;

namespace Vistony.Worker.Infrastructure.Firestore
{
    public class FireStoreClientes : IFirestoreMigrar
    {
        private readonly CollectionReference _customersRef;

        public FireStoreClientes(IConfiguration configuration)
        {
            var db = new FirestoreDbBuilder
            {
                ProjectId = configuration["FirestoreCliente:ProjectId"],
                CredentialsPath = configuration["FirestoreCliente:KeyPath"]
            }.Build();

            _customersRef = db.Collection("customers");
        }

        public async Task RegistrarAsync(ClienteMigracion cliente)
        {
            var item = new
            {
                codeSap = cliente.CodeSap,
                identificationCode = cliente.IdentificationCode,
                name = cliente.Name,
                street = cliente.Street,
                block = cliente.Block,
                department = cliente.Department,
                latitude = cliente.Latitude,
                longitude = cliente.Longitude,
                country = cliente.Country,
                created = Timestamp.FromDateTime(DateTime.SpecifyKind(cliente.Created, DateTimeKind.Utc))
            };

            await _customersRef.AddAsync(item);
        }
    }
}

using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Grpc.Auth;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.Articulo.Interfaces;

namespace Vistony.Worker.Infrastructure.Firestore
{
    public class FirestoreArticuloClient : IFirestoreArticuloClient
    {
        private readonly FirestoreDb _db;

        public FirestoreArticuloClient(IConfiguration configuration)
        {
            var projectId = configuration["Firestore:ProjectId"]!;
            var keyPath = configuration["Firestore:KeyPath"]!;

            GoogleCredential credential =
                GoogleCredential.FromFile(keyPath);

            _db = new FirestoreDbBuilder
            {
                ProjectId = projectId,
                ChannelCredentials = credential.ToChannelCredentials()
            }.Build();
        }

        public async Task<HashSet<string>> ObtenerItemCodesExistentesAsync(string companyId)
        {
            var snapshot = await _db.Collection("items")
                .WhereEqualTo("companyId", companyId)
                .Select("itemCode")
                .GetSnapshotAsync();

            var result = new HashSet<string>();

            foreach (var doc in snapshot.Documents)
            {
                if (doc.TryGetValue<string>("itemCode", out var code) && !string.IsNullOrWhiteSpace(code))
                    result.Add(code);
            }

            return result;
        }

        public async Task<string?> ObtenerCompanyIdAsync(string location)
        {
            Query query = _db
                .Collection("companies")
                .WhereEqualTo("code", location);

            QuerySnapshot snapshot =
                await query.GetSnapshotAsync();

            if (snapshot.Documents.Count == 0)
                return null;

            return snapshot.Documents.First().Id;
        }

        public async Task<bool> ExisteAsync(
            string companyId,
            string itemCode)
        {
            Query query = _db
                .Collection("items")
                .WhereEqualTo("companyId", companyId)
                .WhereEqualTo("itemCode", itemCode);

            QuerySnapshot snapshot =
                await query.GetSnapshotAsync();

            return snapshot.Documents.Count > 0;
        }

        public async Task GuardarAsync(
            Domain.Articulo.Articulo articulo)
        {
            var item = new
            {
                companyId = articulo.CompanyId,
                corpLine = articulo.CorpLine,
                inventoryWeight = articulo.InventoryWeight,
                itemCode = articulo.ItemCode,
                itemName = articulo.ItemName,
                itemsGroupCode = articulo.ItemsGroupCode,
                purchaseUnitHeight = articulo.PurchaseUnitHeight,
                purchaseUnitLength = articulo.PurchaseUnitLength,
                purchaseUnitWidth = articulo.PurchaseUnitWidth,
                qtyPallet = articulo.QtyPallet,
                uoMGroupEntry = articulo.UoMGroupEntry
            };

            await _db
                .Collection("items")
                .AddAsync(item);
        }
    }
}

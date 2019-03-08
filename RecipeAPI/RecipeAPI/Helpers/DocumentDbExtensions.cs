﻿using Microsoft.ApplicationInsights;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using RecipeAPI.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace RecipeAPI.Helpers
{
    /// <summary>
    /// Helps abstract common functionalities of the document db api.
    /// </summary>
    public static class DocumentDbExtensions
    {
        /// <summary>
        /// Deserializes a document into the specified template object type
        /// </summary>
        public static async Task<T> ReadDocument<T>(this DocumentClient client, string databaseName, string collectionName, string documentId)
            where T : IDocument
        {
            await client.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseName });
            await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(databaseName), new DocumentCollection { Id = collectionName });

            try
            {
                var uri = UriFactory.CreateDocumentUri(databaseName, collectionName, documentId);
                var result = await client.ReadDocumentAsync(uri);

                var doc = (T)(dynamic)result.Resource;
                doc.Id = result.Resource.Id;

                return doc;
            }
            catch(DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    return default(T);
                }
                else
                {
                    new TelemetryClient().LogCustomEvent(de.GetType().FullName,
                                                         new KeyValuePair<string, string>("Message", de.Message));
                    return default(T);
                }
            }
            catch(InvalidCastException ex)
            {
                new TelemetryClient().LogCustomEvent(ex.GetType().FullName,
                                                     new KeyValuePair<string, string>("Message", ex.Message),
                                                     new KeyValuePair<string,string>("StackTrace", ex.StackTrace));
                return default(T);
            }
            catch (Exception ex)
            {
                new TelemetryClient().LogCustomEvent(ex.GetType().FullName,
                                                     new KeyValuePair<string, string>("Message", ex.Message),
                                                     new KeyValuePair<string, string>("StackTrace", ex.StackTrace));
                return default(T);
            }
        }

        /// <summary>
        /// Adds a new or updates an existing document in the db.
        /// </summary>
        public static async Task<Document> AddOrUpdateDocument<T>(this DocumentClient client, string databaseName, string collectionName, T document)
            where T : IDocument
        {
            await client.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseName });
            await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(databaseName), new DocumentCollection { Id = collectionName });

            return await client.CreateDocumentIfNotExists(databaseName, collectionName, document);
        }

        private static async Task<Document> CreateDocumentIfNotExists<T>(this DocumentClient client, string databaseName, string collectionName, T document) 
            where T : IDocument
        {
            // TODO: Move away from exception based logic here. There should be something more performant.
            try
            {
                var uri = UriFactory.CreateDocumentUri(databaseName, collectionName, document.Id ?? "Default");
                await client.ReadDocumentAsync(uri);
                await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, document.Id ?? string.Empty), document);
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    return await client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), document);
                }
                else
                {
                    new TelemetryClient().LogCustomEvent(de.GetType().FullName,
                                                         new KeyValuePair<string, string>("Message", de.Message));
                    throw;
                }
            }
            catch (Exception ex)
            {
                new TelemetryClient().LogCustomEvent(ex.GetType().FullName,
                                                     new KeyValuePair<string, string>("Message", ex.Message),
                                                     new KeyValuePair<string, string>("StackTrace", ex.StackTrace));
                return null;
            }

            return null;
        }
    }
}

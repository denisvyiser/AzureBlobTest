using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzureBlobTest
{
    class Program
    {
        static async Task Main(string[] args)
        {

            string connectionString = "DefaultEndpointsProtocol=https;AccountName=event;AccountKey=Z1yN9gjZltd0E0f9u+FVCIVV2890L3VS2SpsYaK/9n46anFTAHXBtH+dwuIS0o058BhbX/UhJZ/AjpDaKgKFzg==;EndpointSuffix=core.windows.net";
            string containerName = "imagesapi";
            string localFilePath = "C:\\Tipos de Filas_RabbitMQ.png";

            //Upload from diretório
            string bloboname = await UploadBlob(connectionString, containerName, localFilePath);


            using FileStream uploadFileStream = File.OpenRead(localFilePath);

            //Upload from Base64

            await UploadBlobBase64(connectionString, containerName, await EncodeTo64FromFilePath(localFilePath), localFilePath.Split(".")[1]);

            //Upload from Stream


            await UploadBlobStream(connectionString, containerName, uploadFileStream, localFilePath.Split(".")[1]);

        }

        public static async Task<string> UploadBlob(string connectionString, string containerName, string localFilePath)
        {

            string bloboName = $"{Guid.NewGuid().ToString()}.{(localFilePath.Split(".")[1])}";

            //Criar client de conexao
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);


            BlobContainerClient blobClient = blobServiceClient.GetBlobContainerClient(containerName);


            using FileStream uploadFileStream = File.OpenRead(localFilePath);

            await blobClient.UploadBlobAsync(bloboName, uploadFileStream);
            uploadFileStream.Close();

            return bloboName;

        }

        public static async Task<string> UploadBlobBase64(string connectionString, string containerName, string filebase64, string extensao)
        {

            string bloboName = $"{Guid.NewGuid().ToString()}.{extensao}";

            //Criar client de conexao
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);


            BlobContainerClient blobClient = blobServiceClient.GetBlobContainerClient(containerName);


            var bytes = Convert.FromBase64String(filebase64);

            await blobClient.UploadBlobAsync(bloboName, new MemoryStream(bytes));
            
            return bloboName;

        }

        public static async Task<string> UploadBlobStream(string connectionString, string containerName, FileStream FileStream, string extensao)
        {

            string bloboName = $"{Guid.NewGuid().ToString()}.{extensao}";

            //Criar client de conexao
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);


            BlobContainerClient blobClient = blobServiceClient.GetBlobContainerClient(containerName);

          

                await blobClient.UploadBlobAsync(bloboName, FileStream);
           
            
            return bloboName;

        }

        static public async Task<string> EncodeTo64FromFilePath(string localFilePath)

        {

            using FileStream uploadFileStream = File.OpenRead(localFilePath);

            using (MemoryStream m = new MemoryStream())
                {
                uploadFileStream.CopyToAsync(m);

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(m.ToArray());
                    return base64String;
                }
            

        }


        static public async Task<string> EncodeTo64FromStream(FileStream FileStream)

        {
           
            using (MemoryStream m = new MemoryStream())
                {

                await FileStream.CopyToAsync(m);
                                       // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(m.ToArray());
                    return base64String;
                }
        
        }

        

    }
}

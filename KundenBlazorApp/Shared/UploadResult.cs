using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;

namespace KundenBlazorApp.Shared
{
    public class UploadResult
    {
        public bool Uploaded { get; set; }
        public IBrowserFile? File { get; set; }
        public string? FileName { get; set; }
        public string? StoredFileName { get; set; }
        public int ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public static int FileMaxSize { get; } = 1024 * 1024 * 1;
        public static int MaxAllowedFiles { get; } = 4;

        public static async Task<UploadResult?> UploadFile(HttpClient httpClient,
            UploadResult uploadResult)
        {
            var upload = false;
            long maxFileSize = FileMaxSize;
            if (uploadResult is not null && uploadResult.File is not null)
            {
                using var content = new MultipartFormDataContent();
                var file = uploadResult.File;
                try
                {
                    var fileContent =
                        new StreamContent(file.OpenReadStream(maxFileSize));

                    fileContent.Headers.ContentType =
                        new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

                    content.Add(
                        content: fileContent,
                        name: "\"files\"",
                        fileName: file.Name);

                    upload = true;
                }
                catch (Exception ex)
                {
                    uploadResult.ErrorCode = 6;
                    uploadResult.ErrorMessage = ex.Message;
                    uploadResult.Uploaded = false;
                }
                if (upload)
                {
                    var response = await httpClient.PostAsync($"api/PicSave", content);

                    var newUploadResults = await response.Content
                        .ReadFromJsonAsync<IList<UploadResult>>();

                    return newUploadResults[0];
                }
            }
            return uploadResult;
        }


    }
}

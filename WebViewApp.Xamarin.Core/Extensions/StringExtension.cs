using System;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace WebViewApp.Xamarin.Core.Extensions
{
    public static class StringExtension
    {
        public static string RemoveNewLines(this string stringVal)
        {
            string formattedText = string.Empty;

            formattedText = stringVal?.Replace(System.Environment.NewLine, string.Empty);

            return formattedText;
        }

        public static long ToNumber(this string stringVal)
        {
            bool canParse = long.TryParse(stringVal, out long numberValue);

            return numberValue;
        }

        public static bool Search(this string stringVal, string text)
        {
            if (string.IsNullOrEmpty(stringVal) || string.IsNullOrEmpty(text))
            {
                return false;
            }

            bool contains = stringVal.ToLower().Contains(text.ToLower());

            return contains;
        }

        public static bool Match(this string stringVal, string text)
        {
            if (string.IsNullOrEmpty(stringVal) || string.IsNullOrEmpty(text))
            {
                return false;
            }

            bool equals = stringVal.Equals(text, StringComparison.InvariantCultureIgnoreCase);

            return equals;
        }

        public static bool IsURI(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            Uri uriResult;
            bool isURI = Uri.TryCreate(text, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return isURI;
        }

        public static string RemoveInvalidText(this string stringVal)
        {
            //emojies
            string text = Regex.Replace(stringVal, @"[^\u0000-\u007F]+", string.Empty);

            //html
            text = Regex.Replace(text, "<.*?>", string.Empty);

            return text.Trim();
        }

        public static string GetExtension(this string mimeType)
        {
            string extension = ".file";

            if (!string.IsNullOrEmpty(mimeType))
            {
                switch (mimeType.ToLower())
                {
                    case "text/plain":
                        extension = ".txt";
                        break;
                    case "application/msword":
                        extension = ".doc";
                        break;
                    case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                        extension = ".docx";
                        break;
                    case "application/pdf":
                        extension = ".pdf";
                        break;
                    case "application/vnd.ms-excel":
                        extension = ".xls";
                        break;
                    case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                        extension = ".xlsx";
                        break;
                    case "image/jpeg":
                        extension = ".jpg";
                        break;
                    case "image/jpg":
                        extension = ".jpg";
                        break;
                    case "image/png":
                        extension = ".png";
                        break;
                    case "image/gif":
                        extension = ".gif";
                        break;
                    case "text/html":
                        extension = ".html";
                        break;
                    case "text/csv":
                        extension = ".csv";
                        break;
                    case "application/xml":
                        extension = ".xml";
                        break;
                    case "application/json":
                        extension = ".json";
                        break;
                    case "application/zip":
                        extension = ".zip";
                        break;
                    case "application/x-rar-compressed":
                        extension = ".rar";
                        break;
                    case "application/x-7z-compressed":
                        extension = ".7z";
                        break;
                    case "audio/mpeg":
                        extension = ".mp3";
                        break;
                    case "audio/wav":
                        extension = ".wav";
                        break;
                    case "video/mp4":
                        extension = ".mp4";
                        break;
                    case "video/x-msvideo":
                        extension = ".avi";
                        break;
                    case "video/x-matroska":
                        extension = ".mkv";
                        break;
                    case "video/quicktime":
                        extension = ".mov";
                        break;
                    case "application/vnd.ms-powerpoint":
                        extension = ".ppt";
                        break;
                    case "application/vnd.openxmlformats-officedocument.presentationml.presentation":
                        extension = ".pptx";
                        break;
                    case "image/svg+xml":
                        extension = ".svg";
                        break;
                    case "application/x-tar":
                        extension = ".tar";
                        break;
                    case "application/rtf":
                        extension = ".rtf";
                        break;
                    default:
                        extension = ".file";
                        break;
                }
            }

            return extension;
        }

        public static string GetMimeType(this string extension)
        {
            string mimeType = "application/octet-stream"; // Default MIME type if none is found

            if (!string.IsNullOrEmpty(extension))
            {
                switch (extension.ToLower())
                {
                    case ".txt":
                        mimeType = "text/plain";
                        break;
                    case ".doc":
                        mimeType = "application/msword";
                        break;
                    case ".docx":
                        mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    case ".pdf":
                        mimeType = "application/pdf";
                        break;
                    case ".xlsx":
                    case ".xls":
                        mimeType = "application/vnd.ms-excel";
                        break;
                    case ".jpeg":
                    case ".jpg":
                        mimeType = "image/jpeg";
                        break;
                    case ".png":
                        mimeType = "image/png";
                        break;
                    case ".gif":
                        mimeType = "image/gif";
                        break;
                    case ".html":
                    case ".htm":
                        mimeType = "text/html";
                        break;
                    case ".csv":
                        mimeType = "text/csv";
                        break;
                    case ".xml":
                        mimeType = "application/xml";
                        break;
                    case ".json":
                        mimeType = "application/json";
                        break;
                    case ".zip":
                        mimeType = "application/zip";
                        break;
                    case ".rar":
                        mimeType = "application/x-rar-compressed";
                        break;
                    case ".7z":
                        mimeType = "application/x-7z-compressed";
                        break;
                    case ".mp3":
                        mimeType = "audio/mpeg";
                        break;
                    case ".wav":
                        mimeType = "audio/wav";
                        break;
                    case ".mp4":
                        mimeType = "video/mp4";
                        break;
                    case ".avi":
                        mimeType = "video/x-msvideo";
                        break;
                    case ".mkv":
                        mimeType = "video/x-matroska";
                        break;
                    case ".mov":
                        mimeType = "video/quicktime";
                        break;
                    case ".ppt":
                        mimeType = "application/vnd.ms-powerpoint";
                        break;
                    case ".pptx":
                        mimeType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                        break;
                    case ".svg":
                        mimeType = "image/svg+xml";
                        break;
                    case ".tar":
                        mimeType = "application/x-tar";
                        break;
                    case ".rtf":
                        mimeType = "application/rtf";
                        break;
                    default:
                        mimeType = "application/octet-stream"; // Default binary file type
                        break;
                }
            }

            return mimeType;
        }

    }
}

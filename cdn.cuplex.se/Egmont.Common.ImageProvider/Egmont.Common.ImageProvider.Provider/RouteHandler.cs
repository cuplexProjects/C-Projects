using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Routing;
using Cuplex.Common.ImageProvider.Configuration;

namespace Cuplex.Common.ImageProvider
{
	/// <summary>
	/// Class RouteHandler.
	/// </summary>
	/// <seealso cref="System.Web.Routing.IRouteHandler" />
	/// <seealso cref="System.Web.IHttpHandler" />
	internal class RouteHandler : IRouteHandler, IHttpHandler
	{
		private readonly ImageProviderPath _imageProviderPath;
	    private readonly GeneralApplicationConfig _applicationConfig;

		/// <summary>
		/// Initializes a new instance of the <see cref="RouteHandler"/> class.
		/// </summary>
		/// <param name="imageProviderPath">The image provider path.</param>
		public RouteHandler(ImageProviderPath imageProviderPath)
		{
			_imageProviderPath = imageProviderPath;
		    _applicationConfig= new GeneralApplicationConfig();

        }

		#region Implementation of IRouteHandler

		/// <summary>
		/// Provides the object that processes the request.
		/// </summary>
		/// <param name="requestContext">An object that encapsulates information about the request.</param>
		/// <returns>An object that processes the request.</returns>
		public IHttpHandler GetHttpHandler(RequestContext requestContext) => this;

		#endregion

		#region Implementation of IHttpHandler

		/// <summary>
		/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Clear();

			var path = context.Request.RequestContext?.RouteData?.Values?["path"] as string;
			if (string.IsNullOrWhiteSpace(path))
			{
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
				return;
			}

			DateTime? ifModifiedSince = null;
			var ifModifiedSinceHeader = context.Request.Headers["If-Modified-Since"];
			if (!string.IsNullOrEmpty(ifModifiedSinceHeader))
			{
				if (DateTime.TryParse(ifModifiedSinceHeader, out DateTime tmp))
				{
					ifModifiedSince = tmp;
				}
			}

			string originalFile;

		    if (_applicationConfig.EnableFluentImage)
		    {
                originalFile = path.Substring(0, path.LastIndexOf(path.Contains("__") ? "__" : ".", StringComparison.Ordinal)) + Path.GetExtension(path);

            }
		    else
		    {
		        originalFile = path;

		    }
           
			var originalPath = Path.Combine(_imageProviderPath.OriginalPath, originalFile).Replace('/', '\\');
			var originalFileInfo = new FileInfo(originalPath);

			var localPath = Path.Combine(_imageProviderPath.LocalPath, path).Replace('/', '\\');
			var localFileInfo = new FileInfo(localPath);

			// Does the (possibly scaled) file already exist?
			if (localFileInfo.Exists)
			{
				// If the original file is newer than the scaled file - the scaled file needs to be re-created.
				if (originalFileInfo.Exists && originalFileInfo.LastWriteTime <= localFileInfo.LastWriteTime)
				{
					if (ifModifiedSince.HasValue && localFileInfo.LastWriteTime <= ifModifiedSince)
					{
						// File has not been modified since the client last fetched the file.
						context.Response.StatusCode = (int)HttpStatusCode.NotModified;
						return;
					}

					SetCacheAndSendFile(context, localPath, originalPath, localFileInfo);
					return;
				}
			}

			// No local file exists - check if the original exists.

			// Does the original file exist?
			if (originalFileInfo.Exists)
			{
				if (_applicationConfig.EnableFluentImage &&  path.Contains("__"))
				{
					// Process the original image according to the requested image parameters.
					var result = ImageProcessor.ProcessImage(localPath, originalPath);
					if (result)
					{
						// Update the created and last write dates on the scaled file.
						File.SetCreationTime(localPath, originalFileInfo.CreationTime);
						File.SetLastWriteTime(localPath, originalFileInfo.LastWriteTime);

						SetCacheAndSendFile(context, localPath, originalPath, originalFileInfo);
						return;
					}

					// Something went wrong when trying to scale the image.
					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					return;
				}

				// Send the original, unmodified image.
				SetCacheAndSendFile(context, null, originalPath, originalFileInfo);
				return;
			}

			// Neither scaled nor unscaled versions found - give up...
			context.Response.StatusCode = (int)HttpStatusCode.NotFound;
		}

		/// <summary>
		/// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
		/// </summary>
		/// <value><c>true</c> if this instance is reusable; otherwise, <c>false</c>.</value>
		public bool IsReusable => true;

		#endregion

		private void SetCacheAndSendFile(HttpContext context, string localPath, string originalPath, FileSystemInfo fileInfo)
		{
			SetCache(context.Response.Cache, fileInfo.LastWriteTime);
			context.Response.ContentType = MimeMapping.GetMimeMapping(localPath ?? originalPath);

			// Add a dependency so that the ASP.Net output cache invalidates its cache if either the local or the original file is changed.
			if (localPath != null)
			{
				context.Response.AddFileDependency(localPath);
			}

			context.Response.AddFileDependency(originalPath);

			context.Response.TransmitFile(localPath ?? originalPath);
		}

		private void SetCache(HttpCachePolicy cache, DateTime lastModified)
		{
			// Use ASP.Net output cache.

			cache.SetExpires(DateTime.UtcNow.Add(_imageProviderPath.Expiration));
			cache.SetValidUntilExpires(true);
			cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
			cache.SetCacheability(HttpCacheability.Public);
			cache.SetMaxAge(_imageProviderPath.Expiration);
			cache.SetLastModified(lastModified.ToUniversalTime());
			cache.SetETagFromFileDependencies();
		}
	}
}
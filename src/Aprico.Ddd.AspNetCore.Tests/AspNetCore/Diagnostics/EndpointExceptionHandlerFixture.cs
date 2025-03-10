#region Copyright & License

// Copyright © 2024 - 2025 Aprico Consultants
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Threading;
using System.Threading.Tasks;
using Aprico.AutoFixture.Xunit2;
using Aprico.Ddd;
using AutoFixture.AutoMoq;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Aprico.AspNetCore.Diagnostics;

public abstract class EndpointExceptionHandlerFixture
{
	#region Nested Type: TryHandleAsync

	public class TryHandleAsync : EndpointExceptionHandlerFixture
	{
		[Theory]
		[AutoData<AutoMoqCustomization>]
		public async Task ReturnsFalseWhenExceptionNotHandled(EndpointExceptionHandler handler, Exception exception)
		{
			HttpContext httpContext = new DefaultHttpContext();

			var result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

			result.Should()
				.BeFalse();
		}

		[Theory]
		[AutoData<AutoMoqCustomization>]
		public async Task ReturnsStatus400BadRequestWhenValidationException(EndpointExceptionHandler handler, ValidationException exception)
		{
			HttpContext httpContext = new DefaultHttpContext();

			var result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

			result.Should()
				.BeTrue();
			httpContext.Response.StatusCode.Should()
				.Be(StatusCodes.Status400BadRequest);
		}

		[Theory]
		[AutoData<AutoMoqCustomization>]
		public async Task ReturnsStatus404NotFoundWhenEntityNotFoundException(EndpointExceptionHandler handler, EntityNotFoundException exception)
		{
			HttpContext httpContext = new DefaultHttpContext();

			var result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

			result.Should()
				.BeTrue();
			httpContext.Response.StatusCode.Should()
				.Be(StatusCodes.Status404NotFound);
		}

		[Theory]
		[AutoData<AutoMoqCustomization>]
		public async Task ReturnsStatus500InternalServerErrorWhenInvalidOperationException(EndpointExceptionHandler handler, InvalidOperationException exception)
		{
			HttpContext httpContext = new DefaultHttpContext();

			var result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

			result.Should()
				.BeTrue();
			httpContext.Response.StatusCode.Should()
				.Be(StatusCodes.Status500InternalServerError);
		}
	}

	#endregion
}

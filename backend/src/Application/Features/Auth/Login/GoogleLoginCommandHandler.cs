
using Application.Abstractions.Authentication;
using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Users;
using SharedKernel;

namespace Application.Features.Auth.Login;

public sealed class GoogleLoginCommandHandler(
    IGoogleAuthService googleService,
    IUserRepository _userRepository,
    ITokenProvider tokenProvider) : ICommandHandler<GoogleLoginCommand, GoogleLoginCommandResponse>
{

    async Task<Result<GoogleLoginCommandResponse>> ICommandHandler<GoogleLoginCommand, GoogleLoginCommandResponse>.Handle(GoogleLoginCommand command, CancellationToken cancellationToken)
    {
        GoogleUserInfo googleUser = await googleService.ExchangeCodeAsync(command.Code);

        User? user = await _userRepository.GetByEmailAsync(googleUser.Email);

        if (user == null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                Email = googleUser.Email,
                Name = googleUser.Name,
                GoogleId = googleUser.GoogleId,
                PictureUrl = googleUser.PictureUrl
            };

            await _userRepository.AddAsync(user);
        }
        else
        {
            user.Name = googleUser.Name;
        }

        await _userRepository.SaveChangesAsync();

        return Result.Success(new GoogleLoginCommandResponse(tokenProvider.Create(user)));
    }
}

﻿using Microsoft.Extensions.DependencyInjection;
using Sample.Factories;
using SimpleCommander.Commands;
using SimpleCommander.Services;
using System;
using System.CommandLine;

namespace Sample.Commands.AddTeamToRepo;

public sealed class AddTeamToRepoCommand : CommandBase<AddTeamToRepoCommandArgs, AddTeamToRepoCommandHandler>
{
    public AddTeamToRepoCommand() : base(
        name: "add-team-to-repo",
        description: "Adds a team to a repo with a specific role/permission" +
                     Environment.NewLine +
                     "Note: Expects GH_PAT env variable or --github-pat option to be set.")
    {
        AddOption(GithubOrg);
        AddOption(GithubRepo);
        AddOption(Team);
        AddOption(Role.FromAmong("pull", "push", "admin", "maintain", "triage"));
        AddOption(GithubPat);
        AddOption(Verbose);
    }

    public Option<string> GithubOrg { get; } = new("--github-org")
    {
        IsRequired = true
    };
    public Option<string> GithubRepo { get; } = new("--github-repo")
    {
        IsRequired = true
    };
    public Option<string> Team { get; } = new("--team")
    {
        IsRequired = true
    };
    public Option<string> Role { get; } = new("--role")
    {
        IsRequired = true,
        Description = "The only valid values are: pull, push, admin, maintain, triage. For more details see https://docs.github.com/en/rest/reference/teams#add-or-update-team-repository-permissions, custom repository roles are not currently supported."
    };
    public Option<string> GithubPat { get; } = new("--github-pat");
    public Option<bool> Verbose { get; } = new("--verbose");

    public override AddTeamToRepoCommandHandler BuildHandler(AddTeamToRepoCommandArgs args, IServiceProvider sp)
    {
        if (args is null)
        {
            throw new ArgumentNullException(nameof(args));
        }

        if (sp is null)
        {
            throw new ArgumentNullException(nameof(sp));
        }

        var logger = sp.GetRequiredService<CLILogger>();
        var targetGithubApiFactory = sp.GetRequiredService<GithubApiFactory>();
        var githubApi = targetGithubApiFactory.Create(targetPersonalAccessToken: args.GithubPat);

        return new AddTeamToRepoCommandHandler(logger, githubApi);
    }
}

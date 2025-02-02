// This file is used by Code Analysis to maintain SuppressMessage attributes that are applied to this project.
// Project-level suppressions either have no target or are given a specific target and scoped to a namespace, type,
// member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Maintainability",
    "S3353:Unchanged variables should be marked as \"const\"",
    Justification = "False positive for all Razor @inject statements:" +
        "https://community.sonarsource.com/t/s3353-false-positive-for-razor-inject/134400.",
    Scope = "module")]

read -r -d '' release_description << 'EOF'
NET wrapper package for [safe-api](https://github.com/maidsafe/safe-api/).

## Changelog
CHANGELOG_CONTENT

## SHA-256 checksums:
```
MaidSafe.SafeApp NuGet Package
NUGET_PACKAGE_CHECKSUM
```

## Related Links
* SAFE Browser - [Desktop](https://github.com/maidsafe/safe_browser/releases/) | [Mobile](https://github.com/maidsafe/safe-mobile-browser/)
* [SAFE Mobile Authenticator](https://github.com/maidsafe/safe-authenticator-mobile/)
* [SAFE CLI](https://github.com/maidsafe/safe-api/tree/master/safe-cli)
* [SAFE Vault](https://github.com/maidsafe/safe_vault/releases/latest/)
EOF

commitMessage=$(git log --format=%B -n 1)
version=$(perl -pe '($_)=/([0-9]+([.][0-9]+)+([-][Rr][Cc][0-9]+)?)/' <<< $commitMessage)
nuget_package_checksum=$(sha256sum "../MaidSafe.SafeApp.${version}.nupkg" | awk '{ print $1 }')
changelog_content=$(sed '1,/### Changes/d;/##/,$d' ../CHANGELOG.MD)
release_description=$(sed "s/NUGET_PACKAGE_CHECKSUM/$nuget_package_checksum/g" <<< "$release_description")

echo "${release_description/CHANGELOG_CONTENT/$changelog_content}" > release_description.txt

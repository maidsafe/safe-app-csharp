#!/usr/bin/env bash

archive_name=alpha-2.zip
android_dst=SafeApp.AppBindings.Android/lib
ios_dst=SafeApp.AppBindings.iOS/lib

cd ..

curl -o $archive_name "https://s3.eu-west-2.amazonaws.com/safe-app-csharp/$archive_name"

rm -rf $android_dst
rm -rf $ios_dst

unzip -q $archive_name

mv android/lib $android_dst
mv ios/lib $ios_dst

rm -rf android
rm -rf ios

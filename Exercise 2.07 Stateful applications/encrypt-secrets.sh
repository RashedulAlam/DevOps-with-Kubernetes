#!/usr/bin/env bash

[ -f "secret.txt" ] && rm "secret.txt"

age-keygen -o secret.txt

SECRET_FILE=$( sed -n '2p' secret.txt | cut -d ":" -f 2 | cut -d " " -f 2 )

# encrypt the yaml file
sops --encrypt --age $SECRET_FILE --encrypted-regex '^(data)$' manifests/secret/secret.yaml > manifests/secret/secret.enc.yaml

cat manifests/secret/secret.enc.yaml

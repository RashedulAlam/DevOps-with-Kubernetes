#!/usr/bin/env bash

cd "TodoApp"
docker build -t todoapp:2.0 .

cd ..

cd "TodoService"
docker build -t todoservice:2.0 .

cd ..

# encrpyt the secret file
./encrypt-secrets.sh

# set the secret key
export SOPS_AGE_KEY_FILE=$(pwd)/secret.txt

k3d cluster delete ex-208 && k3d cluster create ex-208 --api-port 127.0.0.1:6443 -p 8081:80@loadbalancer --agents 1

k3d image import todoapp:2.0 --cluster ex-208 && k3d image import todoservice:2.0  --cluster ex-208

winpty docker exec -it k3d-ex-208-agent-0 sh -c "mkdir -p /tmp/kube && exit"

kubectl apply -f ./manifests/namespace/namespace.yaml

# apply the decrypted file
sops --decrypt manifests/secret/secret.enc.yaml | kubectl apply -f -

kubectl apply -f ./manifests/configMaps/configMap.yaml
kubectl apply -f ./manifests/volume/pv.yaml
kubectl apply -f ./manifests/pvc/pvc-logger.yaml
kubectl apply -f ./manifests/deployment/deployment.yaml
kubectl apply -f ./manifests/deployment/service.yaml
kubectl apply -f ./manifests/deployment/database.yaml
kubectl apply -f ./manifests/deployment/ingress.yaml
kubectl apply -f ./manifests/statefulSet/database.yaml
kubectl apply -f ./manifests/cronjob/cronjob.yaml

kubectl get all -n ex-208

./install-monitoring.sh
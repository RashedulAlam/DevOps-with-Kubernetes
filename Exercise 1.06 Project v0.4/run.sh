
k3d cluster delete ex-106 && k3d cluster create ex-106 --api-port 127.0.0.1:6443 --port 8082:30080@agent:0 -p 8081:80@loadbalancer --agents 2

k3d image import angularapp:1.0 --cluster ex-106

kubectl apply -f "../Exercise 1.05 Project v0.3/manifests/deployment.yaml"

kubectl apply -f ./manifests/service.yaml

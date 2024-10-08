k3d cluster delete ex-000 && k3d cluster create ex-000 --api-port 127.0.0.1:6443 -p 8081:80@loadbalancer --agents 1

kubectl apply -f ./manifests/namespace.yaml

kubectl apply -f ./manifests/statefulSetLearning.yaml

kubectl run busybox --image=busybox -it --rm --restart=Never --namespace=ex-000 -- sh

# k3d cluster create --api-port 127.0.0.1:6443
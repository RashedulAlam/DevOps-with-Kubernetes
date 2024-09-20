# install prometheus
kubectl create namespace prometheus
helm install prometheus-community/kube-prometheus-stack --generate-name --namespace prometheus


kubectl get po -n prometheus

# install loki
kubectl create namespace loki-stack
helm upgrade --install loki --namespace=loki-stack grafana/loki-stack --set loki.image.tag=2.9.3

# https://github.com/grafana/loki/issues/11557 
# issues due to recent upragde of loki image it can not be added to grafana 
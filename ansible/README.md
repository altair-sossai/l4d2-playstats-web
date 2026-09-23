# Ansible – L4D2 PlayStats Web

Provisions the VPS and (re)deploys the L4D2 PlayStats web site. It reproduces the
manual/pipeline setup: a self-contained .NET 10 publish that runs under `screen`
in `/home/l4d2-playstats-web`, listening on port 5000, and relaunched on reboot
through an `@reboot` cron entry.

## What it does

- Installs runtime/deploy dependencies (`screen`, `dos2unix`, `libicu`, `rsync`).
- Creates `/home/l4d2-playstats-web` (and `bash/`) owned by the deploy user.
- Publishes the project (`dotnet publish --self-contained`) and ships the binaries.
- Ships `bash/bootstrap.sh` and normalizes its line endings.
- Renders `appsettings.json` from variables + Ansible Vault secrets.
- Installs an `@reboot` cron job so the app comes back after a restart.
- Starts (or restarts) the app inside a `screen` session.

## Prerequisites

- Ansible on the control node (Linux/WSL/macOS).
- .NET 10 SDK on the control node (only when `web_build_locally: true`).
- SSH access to the VPS as the deploy user with your key.

```bash
ansible-galaxy collection install -r requirements.yml
```

## Configure

1. Secrets:

   ```bash
   cp group_vars/vault.example.yml group_vars/vault.yml
   # fill in the real values, then:
   ansible-vault encrypt group_vars/vault.yml
   ```

2. Non-secret config lives in `group_vars/all.yml`.
3. The target host/user is in `inventory.ini` (`124.198.128.43`, user `devops`).

## Run

```bash
ansible-playbook site.yml --ask-vault-pass
```

Deploy an already-published folder instead of building on the control node:

```bash
ansible-playbook site.yml --ask-vault-pass \
  -e web_build_locally=false -e web_publish_dir=/path/to/publish
```

## Notes

- The app runs via `screen`, matching `scripts/deploy.sh` / `bash/bootstrap.sh`.
  Boot persistence is handled by the `@reboot` cron entry (not systemd).
- To switch to a robust systemd service later, replace the cron + screen tasks
  with a unit file (`Restart=always`, `WantedBy=multi-user.target`).

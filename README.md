# S3 Upload utility

### Usage:

```
S3Sync.exe

-r, --region         (Default: eu-central-1) AWS Region to use, defaults to eu-central-1

-d, --destination    Required. Destination key in S3 bucket. Without leading /

-b, --bucket         Required. Destination bucket name

-s, --source         Required. Source file path

-i, --id             Required. AWS access key id

-p, --secret         Required. AWS access key secret

--help               Display this help screen.

--version            Display version information
```
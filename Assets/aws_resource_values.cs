using System;
[Serializable]
public class AWSResourceValues
{
    public AWSResource authentication_api_key;
    public AWSResource authentication_lambda_url;
    public AWSResource experimentTableArn;
    public AWSResource get_data_api_key;
    public AWSResource get_data_url;
    public AWSResource lambda_bucket_name;
    public AWSResource set_data_sqs_url;
}
[Serializable]
public class AWSResource
{
    public bool sensitive;
    public string type;
    public string value;
}
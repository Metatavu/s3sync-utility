using CommandLine;
using System;
using System.Net.Http;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;

namespace S3Sync
{
    class Program
    {
        public class Options
        {
            [Option('r', "region", Default = "eu-central-1", HelpText = "AWS Region to use, defaults to eu-central-1")]
            public string Region { get; set; }

            [Option('d', "destination", Required = true, HelpText = "Destination key in S3 bucket. Without leading /")]
            public string Destination { get; set; }

            [Option('b', "bucket", Required = true, HelpText = "Destination bucket name")]
            public string Bucket { get; set; }

            [Option('s', "source", Required = true, HelpText = "Source file path")]
            public string Source { get; set; }

            [Option('i', "id", Required = true, HelpText = "AWS access key id")]
            public string AccessKey { get; set; }

            [Option('p', "secret", Required = true, HelpText = "AWS access key secret")]
            public string AccessSecret { get; set; }
        }
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(async options =>
                {
                    try
                    {
                        RegionEndpoint region = RegionEndpoint.GetBySystemName(options.Region);
                        AWSCredentials credentials = new BasicAWSCredentials(options.AccessKey, options.AccessSecret);
                        AmazonS3Client client = new AmazonS3Client(credentials, region);
                        PutObjectRequest request = new PutObjectRequest
                        {
                            BucketName = options.Bucket,
                            Key = options.Destination,
                            FilePath = options.Source
                        };

                        PutObjectResponse response = await client.PutObjectAsync(request);
                        if (!new HttpResponseMessage(response.HttpStatusCode).IsSuccessStatusCode)
                        {
                            throw new Exception($"Error during s3 upload, received status {response.HttpStatusCode}");
                        }
                        Console.WriteLine("File uploaded successfully.");
                    } 
                    catch (Exception e)
                    {
                        Console.WriteLine($"S3 upload failed: {e.Message}");
                        Console.WriteLine(e.StackTrace);
                    }
                });
        }
    }
}

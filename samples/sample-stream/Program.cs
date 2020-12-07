﻿using Streamiz.Kafka.Net;
using Streamiz.Kafka.Net.SerDes;
using Streamiz.Kafka.Net.Stream;
using System;
using System.Threading.Tasks;

namespace sample_stream
{
    /// <summary>
    /// Sample program with a passtrought stream, instanciate and dispose with CTRL+ C console event.
    /// If you want an example with token source passed to startasync, see <see cref="ProgramToken"/> class.
    /// </summary>
    internal class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var config = new StreamConfig<StringSerDes, StringSerDes>();
            config.ApplicationId = "test-app";
            config.BootstrapServers = "192.168.56.1:9092";
            config.SaslMechanism = Confluent.Kafka.SaslMechanism.Plain;
            config.SecurityProtocol = Confluent.Kafka.SecurityProtocol.SaslPlaintext;
            config.SaslUsername = "admin";
            config.SaslPassword = "admin";

            StreamBuilder builder = new StreamBuilder();
            IKStream<string, string> kStream = builder.Stream<string, string>("test-topic");

            kStream.Print(Printed<string, string>.ToOut());

            Topology t = builder.Build();
            KafkaStream stream = new KafkaStream(t, config);

            Console.CancelKeyPress += (o, e) => stream.Dispose();

            await stream.StartAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using CryptoTechProject.Boundary;

namespace CryptoTechProject
{
    public class SlackMessageGenerator
        {
            public SlackMessage ToSlackMessage(GetWorkshopsResponse workshops, string user)
            {
                var sessions = workshops.PresentableWorkshops;

                SlackMessage slackMessage = new SlackMessage
                {
                    Blocks = new List<SlackMessage.SlackMessageBlock>()
                };

                slackMessage.Blocks.Add(new SlackMessage.TitleSectionBlock
                {
                    Text = new SlackMessage.SectionBlockText
                    {
                        Type = "mrkdwn",
                        Text = "*Sessions*"
                    }
                });

                AddDivider(slackMessage);

                if (sessions.Length != 0)
                {
                    AddDateHeader(slackMessage, sessions, 0);
                }

                for (int i = 0; i < sessions.Length; i++)
                {
                    DateTimeOffset sessionEndTime =
                        sessions[i].Time.AddMinutes(sessions[i].Duration);

                    string attendanceStatus = "Attend";
                    if (sessions[i].Attendees.Contains(user))
                    {
                        attendanceStatus = "Unattend";
                    }

                    if (sessions[i].Type == "Showcase")
                    {
                        string showcaseText = $"*{sessions[i].Name}*\n" +
                                              $"{sessions[i].Time.ToString("HH:mm")} - {sessionEndTime.ToString("HH:mm")}\n" +
                                              $"{sessions[i].Host}\n";

                        slackMessage.Blocks.Add(new SlackMessage.ShowcaseSectionBlock
                        {
                            Text = new SlackMessage.SectionBlockText
                            {
                                Type = "mrkdwn",
                                Text = showcaseText
                            }
                        });
                    }
                    else
                    {
                        slackMessage.Blocks.Add(new SlackMessage.SectionBlock
                        {
                            Text = new SlackMessage.SectionBlockText
                            {
                                Type = "mrkdwn",
                                Text = $"*{sessions[i].Name}*\n" +
                                       $"{sessions[i].Time.ToString("HH:mm")} - {sessionEndTime.ToString("HH:mm")}\n" +
                                       $"{sessions[i].Host} in {sessions[i].Location}\n" +
                                       $"Current number of attendees: {sessions[i].Attendees.Count}"
                            },
                            Accessory = new SlackMessage.SectionBlock.AccessoryBlock
                            {
                                Text = new SlackMessage.SectionBlockText
                                {
                                    Type = "plain_text",
                                    Text = attendanceStatus
                                },
                                Value = sessions[i].ID
                            }
                        });
                    }

                    if (i < (sessions.Length - 1) && sessions[i].Time.Date != sessions[i + 1].Time.Date)
                    {
                        AddDivider(slackMessage);
                        AddDateHeader(slackMessage, sessions, i + 1);
                    }
                }

                return slackMessage;
            }

            private static void AddDateHeader(SlackMessage slackMessage, PresentableWorkshop[] sessions,
                int sessionIndex)
            {
                slackMessage.Blocks.Add(new SlackMessage.TitleSectionBlock()
                {
                    Text = new SlackMessage.SectionBlockText
                    {
                        Type = "mrkdwn",
                        Text = $"*{sessions[sessionIndex].Time.ToString("dd/MM/yyyy")}*"
                    }
                });
            }

            private static void AddDivider(SlackMessage slackMessage)
            {
                slackMessage.Blocks.Add(new SlackMessage.DividerBlock
                {
                    Type = "divider"
                });
            }
        }
    }

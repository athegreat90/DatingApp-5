import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { Member } from 'src/app/models/member';
import { Message } from 'src/app/models/message';
import { MembersService } from 'src/app/_services/members.service';
import { MessageService } from 'src/app/_services/message.service';

@Component({
    selector: 'app-member-detail',
    templateUrl: './member-detail.component.html',
    styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit
{
    member: Member | undefined;

    @ViewChild('memberTabs', { static: true }) memberTabs!: TabsetComponent;
    activeTab!: TabDirective;
    messages: Message[] = [];

    constructor(private memberService: MembersService, private route: ActivatedRoute, private messageService: MessageService)
    {
    }

    ngOnInit(): void
    {
        this.route.data.subscribe(data => 
        {
            this.member = data.member;
        });

        this.route.queryParams.subscribe(params => 
        {
            params.tab ? this.selectTab(params.tab) : this.selectTab(0);
        });
    }

    loadMember()
    {
        this.memberService.getMember(this.route.snapshot.paramMap.get('username') as string).subscribe(member => 
        {
            this.member = member;
        });
    }

    onTabActivated(data: TabDirective)
    {
        this.activeTab = data;
        if (this.activeTab.heading === 'Messages' && this.messages.length === 0)
        {
            this.loadMessage();
        }
    }

    loadMessage()
    {
        if (this.member)
        {
            this.messageService.getMessageThread(this.member.username).subscribe(messasges => 
            {
                this.messages = messasges;
            });
        }
    }

    selectTab(tabId: number)
    {
        this.memberTabs.tabs[tabId].active = true;
    }

}

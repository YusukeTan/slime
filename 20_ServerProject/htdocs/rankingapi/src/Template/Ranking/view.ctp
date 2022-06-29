<?php
/**
 * @var \App\View\AppView $this
 * @var \App\Model\Entity\Ranking $ranking
 */
?>
<nav class="large-3 medium-4 columns" id="actions-sidebar">
    <ul class="side-nav">
        <li class="heading"><?= __('Actions') ?></li>
        <li><?= $this->Html->link(__('Edit Ranking'), ['action' => 'edit', $ranking->Id]) ?> </li>
        <li><?= $this->Form->postLink(__('Delete Ranking'), ['action' => 'delete', $ranking->Id], ['confirm' => __('Are you sure you want to delete # {0}?', $ranking->Id)]) ?> </li>
        <li><?= $this->Html->link(__('List Ranking'), ['action' => 'index']) ?> </li>
        <li><?= $this->Html->link(__('New Ranking'), ['action' => 'add']) ?> </li>
    </ul>
</nav>
<div class="ranking view large-9 medium-8 columns content">
    <h3><?= h($ranking->Id) ?></h3>
    <table class="vertical-table">
        <tr>
            <th scope="row"><?= __('Name') ?></th>
            <td><?= h($ranking->Name) ?></td>
        </tr>
        <tr>
            <th scope="row"><?= __('Time') ?></th>
            <td><?= h($ranking->Time) ?></td>
        </tr>
        <tr>
            <th scope="row"><?= __('Id') ?></th>
            <td><?= $this->Number->format($ranking->Id) ?></td>
        </tr>
        <tr>
            <th scope="row"><?= __('Date') ?></th>
            <td><?= h($ranking->Date) ?></td>
        </tr>
    </table>
</div>
